namespace Identity.API.Pages.Account.Logout;

internal sealed class LoggedOutViewModel
{
    public string? PostLogoutRedirectUri { get; init; }

    public string? ClientName { get; init; }

    public string? SignOutIframeUrl { get; init; }

    public bool AutomaticRedirectAfterSignOut { get; init; }
}
