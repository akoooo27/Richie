using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.UserManagement;
using Duende.UserManagement.Authentication.Otp;

using Identity.API.Authentication.Otp;
using Identity.API.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account.Login;

[AllowAnonymous]
internal sealed class Index
(
    IIdentityServerInteractionService interaction,
    IOtpSender otpSender,
    OtpChallengeCookie challengeCookie
) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task OnGetAsync(string? returnUrl, CancellationToken cancellationToken)
    {
        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(returnUrl, cancellationToken);

        Input = new InputModel
        {
            Email = context?.LoginHint ?? string.Empty,
            ReturnUrl = returnUrl
        };
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!EmailAddresses.TryNormalize(Input.Email, out EmailAddress? email))
        {
            ModelState.AddModelError("Input.Email", "Enter a valid email address.");

            return Page();
        }

        OtpAddress address = new(OtpChannel.Email, email);

        SendOtpResult result = await otpSender.TrySendOtpAsync(address, cancellationToken);

        switch (result)
        {
            case SendOtpResult.Sent sent:
                challengeCookie.Write
                (
                    response: Response,
                    challenge: new OtpChallenge
                    (
                        Token: sent.Token.Value,
                        Email: email.Value,
                        Name: null
                    ),
                    expiresAtUtc: sent.ExpiresAtUtc
                );
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
}
