namespace Identity.API.Pages.Account.Login;

internal sealed class ViewModel
{
    public bool AllowRememberLogin { get; init; } = LoginOptions.AllowRememberLogin;

    public bool EnableLocalLogin { get; init; } = true;
}
