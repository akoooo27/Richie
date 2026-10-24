using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.Storage.EntityAttributeValue;
using Duende.UserManagement;
using Duende.UserManagement.Authentication.Otp;
using Duende.UserManagement.Profiles;

using Identity.API.Authentication.Otp;
using Identity.API.Database;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account.Verify;

[AllowAnonymous]
internal sealed class Index
(
    IIdentityServerInteractionService interaction,
    IOtpAuthenticator otpAuthenticator,
    IOtpSender otpSender,
    IUserProfileSelfService profiles,
    IEventService events,
    OtpChallengeCookie challengeCookie
) : PageModel
{
    public ViewModel View { get; private set; } = new();

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public IActionResult OnGet(string? returnUrl)
    {
        OtpChallenge? challenge = challengeCookie.Read(Request);

        if (challenge is null)
        {
            return RedirectToPage("/Account/Login/Index", new { returnUrl });
        }

        View = new ViewModel { Email = challenge.Email };
        Input = new InputModel { ReturnUrl = returnUrl };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        OtpChallenge? challenge = challengeCookie.Read(Request);

        if (challenge is null)
        {
            return RedirectToPage("/Account/Login/Index", new { returnUrl = Input.ReturnUrl });
        }

        View = new ViewModel { Email = challenge.Email };

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!PlainTextOtp.TryCreate(Input.Code, out PlainTextOtp? code))
        {
            ModelState.AddModelError("Input.Code", "Enter the 8-character code from the email.");

            return Page();
        }

        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(Input.ReturnUrl, cancellationToken);

        OtpAuthenticationResult result = await otpAuthenticator.TryAuthenticateAsync
        (
            otp: code,
            token: challenge.Token,
            ct: cancellationToken
        );

        if (result is not OtpAuthenticationResult.Success success)
        {
            await events.RaiseAsync
            (
                new UserLoginFailureEvent
                (
                    username: challenge.Email,
                    error: "invalid otp",
                    clientId: context?.Client.ClientId
                ),
                cancellationToken
            );

            ModelState.AddModelError("Input.Code", "That code is invalid or has expired.");

            return Page();
        }

        OtpChallengeCookie.Delete(Response);

        UserProfile? profile = await profiles.TryGetAsync(success.UserSubjectId, cancellationToken)
                               ?? await CreateProfileAsync
                               (
                                   subjectId: success.UserSubjectId,
                                   challenge: challenge,
                                   cancellationToken: cancellationToken
                               );

        if (profile is null)
        {
            ModelState.AddModelError(string.Empty, "We couldn't set up your account. Please try again.");

            return Page();
        }

        string displayName = GetName(profile) ?? challenge.Email;

        await HttpContext.SignInAsync(new IdentityServerUser(success.UserSubjectId.Value)
        {
            DisplayName = displayName,
            AuthenticationMethods = { OidcConstants.AuthenticationMethods.OneTimePassword }
        });

        await events.RaiseAsync
        (
            new UserLoginSuccessEvent
            (
                username: challenge.Email,
                subjectId: success.UserSubjectId.Value,
                name: displayName,
                clientId: context?.Client.ClientId
            ),
            cancellationToken
        );

        if (context is not null)
        {
            return Redirect(Input.ReturnUrl!);
        }

        return LocalRedirect(Url.IsLocalUrl(Input.ReturnUrl) ? Input.ReturnUrl : "~/");
    }

    public async Task<IActionResult> OnPostResendAsync(CancellationToken cancellationToken)
    {
        OtpChallenge? challenge = challengeCookie.Read(Request);

        if (challenge is null)
        {
            return RedirectToPage("/Account/Login/Index", new { returnUrl = Input.ReturnUrl });
        }

        View = new ViewModel { Email = challenge.Email };

        ModelState.Clear();

        OtpAddress address = new(OtpChannel.Email, EmailAddress.Create(challenge.Email));

        SendOtpResult result = await otpSender.TrySendOtpAsync(address, cancellationToken);

        switch (result)
        {
            case SendOtpResult.Sent sent:
                challengeCookie.Write
                (
                    response: Response,
                    challenge: challenge with { Token = sent.Token.Value },
                    expiresAtUtc: sent.ExpiresAtUtc
                );
                StatusMessage = "We sent you a new code.";
                return RedirectToPage("/Account/Verify/Index", new { returnUrl = Input.ReturnUrl });

            case SendOtpResult.Blocked blocked:
                ModelState.AddModelError(string.Empty,
                    $"Please wait {Math.Ceiling(blocked.SendingBlockedFor.TotalSeconds)} seconds before requesting a new code.");
                return Page();

            default:
                ModelState.AddModelError(string.Empty,
                    "We couldn't send a code right now. Please try again.");
                return Page();
        }
    }

    private async Task<UserProfile?> CreateProfileAsync
    (
        UserSubjectId subjectId,
        OtpChallenge challenge,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyAttributeSchema schema = await profiles.GetSchemaAsync(cancellationToken);
        AttributeValueCollection attributes = new(schema);

        attributes.Set(UserProfileAttributes.Email.Code, challenge.Email);

        if (!string.IsNullOrWhiteSpace(challenge.Name))
        {
            attributes.Set(UserProfileAttributes.Name.Code, challenge.Name);
        }

        return await profiles.TryCreateAsync
        (
            subjectId: subjectId,
            attributes: attributes.Validate(),
            ct: cancellationToken
        );
    }

    private static string? GetName(UserProfile profile)
    {
        return profile.Attributes.TryGetValue(UserProfileAttributes.Name.Code, out AttributeValue? name) &&
               name.TryGetValue(out string? value)
            ? value
            : null;
    }
}
