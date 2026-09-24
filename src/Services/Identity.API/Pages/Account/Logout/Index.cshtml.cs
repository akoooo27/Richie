using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Identity.API.Database.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account.Logout;

[AllowAnonymous]
internal sealed class Index
(
    SignInManager<ApplicationUser> signInManager,
    IIdentityServerInteractionService interaction,
    IEventService events
) : PageModel
{
    [BindProperty]
    public string? LogoutId { get; set; }

    public async Task<IActionResult> OnGetAsync(string? logoutId, CancellationToken ct)
    {
        LogoutId = logoutId;

        bool showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

        if (User.Identity?.IsAuthenticated != true)
        {
            showLogoutPrompt = false;
        }
        else
        {
            LogoutRequest? context = await interaction.GetLogoutContextAsync(LogoutId, ct);

            if (context?.ShowSignoutPrompt == false)
            {
                showLogoutPrompt = false;
            }
        }

        if (!showLogoutPrompt)
        {
            return await OnPostAsync(ct);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            LogoutId ??= await interaction.CreateLogoutContextAsync(ct);

            await signInManager.SignOutAsync();

            await events.RaiseAsync
            (
                new UserLogoutSuccessEvent
                (

                    subjectId: User.GetSubjectId(),
                    name: User.GetDisplayName()
                ),
                ct
            );
        }

        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
    }
}
