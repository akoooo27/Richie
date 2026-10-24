using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Home.Error;

[AllowAnonymous]
internal sealed class Index
(
    IIdentityServerInteractionService interaction,
    IWebHostEnvironment environment
) : PageModel
{
    public ErrorMessage? Error { get; private set; }

    public async Task OnGetAsync(string? errorId, CancellationToken ct)
    {
        Error = await interaction.GetErrorContextAsync(errorId, ct);

        if (Error is not null && !environment.IsDevelopment())
        {
            Error.ErrorDescription = null;
        }
    }
}
