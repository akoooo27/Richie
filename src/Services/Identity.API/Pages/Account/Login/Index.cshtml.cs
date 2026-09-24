using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Identity.API.Database.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Identity.API.Pages.Account.Login;

[AllowAnonymous]
internal sealed class Index
(
    IIdentityServerInteractionService interaction,
    IEventService events,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
) : PageModel
{
    private const string LoginButton = "login";

    public ViewModel View { get; private set; } = new();

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task OnGetAsync(string? returnUrl, CancellationToken ct)
    {
        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(returnUrl, ct);

        Input = new InputModel
        {
            ReturnUrl = returnUrl,
            Username = context?.LoginHint
        };

        View = BuildViewModel(context);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(Input.ReturnUrl, ct);

        if (Input.Button != LoginButton)
        {
            return await CancelAsync(context, ct);
        }

        if (context is null && !string.IsNullOrEmpty(Input.ReturnUrl) && !Url.IsLocalUrl(Input.ReturnUrl))
        {
            return RedirectToPage("/Home/Error/Index");
        }

        View = BuildViewModel(context);

        if (!View.EnableLocalLogin || !ModelState.IsValid)
        {
            return Page();
        }

        bool rememberLogin = View.AllowRememberLogin && Input.RememberLogin;

        SignInResult result = await signInManager.PasswordSignInAsync
        (
            userName: Input.Username!,
            password: Input.Password!,
            isPersistent: rememberLogin,
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            ApplicationUser user = await userManager.FindByNameAsync(Input.Username!)
                ?? throw new InvalidOperationException("The user signed in but could not be found.");

            string subjectId = await userManager.GetUserIdAsync(user);

            await events.RaiseAsync
            (
                new UserLoginSuccessEvent
                (
                    username: user.UserName,
                    subjectId: subjectId, user.UserName,
                    clientId: context?.Client.ClientId
                ),
                ct
            );

            return RedirectAfterLogin(context);
        }

        await events.RaiseAsync
        (
            new UserLoginFailureEvent
            (
                username: Input.Username,
                error: "invalid credentials",
                clientId: context?.Client.ClientId
            ),
            ct
        );

        ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);

        return Page();
    }

    private async Task<IActionResult> CancelAsync(AuthorizationRequest? context, CancellationToken ct)
    {
        if (context is null)
        {
            return RedirectToPage("/Index");
        }

        // Send an access_denied response back to the client, even when it does not require consent.
        await interaction.DenyAuthorizationAsync
        (
            request: context,
            error: InteractionError.AccessDenied,
            ct: ct
        );

        return RedirectToAuthorizationRequest(context);
    }

    private IActionResult RedirectAfterLogin(AuthorizationRequest? context)
    {
        if (context is not null)
        {
            return RedirectToAuthorizationRequest(context);
        }

        if (string.IsNullOrEmpty(Input.ReturnUrl))
        {
            return RedirectToPage("/Index");
        }

        // Validated as a local URL before signing in.
        return Redirect(Input.ReturnUrl);
    }

    private IActionResult RedirectToAuthorizationRequest(AuthorizationRequest context)
    {
        // The return URL can be trusted because IdentityServer resolved an authorization context from it.
        string returnUrl = Input.ReturnUrl
            ?? throw new InvalidOperationException("An authorization context requires a return URL.");

        return context.IsNativeClient() ? this.LoadingPage(returnUrl) : Redirect(returnUrl);
    }

    private static ViewModel BuildViewModel(AuthorizationRequest? context)
    {
        return new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = context?.Client.EnableLocalLogin ?? true
        };
    }
}
