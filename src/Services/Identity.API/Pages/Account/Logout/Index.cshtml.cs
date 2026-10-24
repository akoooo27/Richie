using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account.Logout;

[AllowAnonymous]
internal sealed class Index(IIdentityServerInteractionService interaction, IEventService events) : PageModel
{
    [BindProperty]
    public string? LogoutId { get; set; }

    public async Task<IActionResult> OnGetAsync(string? logoutId, CancellationToken cancellationToken)
    {
        LogoutId = logoutId;

        if (User.Identity?.IsAuthenticated is not true)
        {
            return await OnPostAsync(cancellationToken);
        }

        LogoutRequest? context = await interaction.GetLogoutContextAsync(LogoutId, cancellationToken);

        if (context?.ShowSignoutPrompt == false)
        {
            return await OnPostAsync(cancellationToken);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated is true)
        {
            LogoutId ??= await interaction.CreateLogoutContextAsync(cancellationToken);

            await HttpContext.SignOutAsync();

            await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()),
                cancellationToken);
        }

        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
    }
}
