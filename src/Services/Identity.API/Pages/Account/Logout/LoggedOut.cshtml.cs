using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account.Logout;

[AllowAnonymous]
internal sealed class LoggedOut(IIdentityServerInteractionService interaction) : PageModel
{
    public LoggedOutViewModel View { get; private set; } = new();

    public async Task OnGetAsync(string? logoutId, CancellationToken cancellationToken)
    {
        LogoutRequest? logout = await interaction.GetLogoutContextAsync(logoutId, cancellationToken);

        View = new LoggedOutViewModel
        {
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl
        };
    }
}
