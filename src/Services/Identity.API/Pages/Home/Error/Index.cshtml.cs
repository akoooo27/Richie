using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Home.Error;

[AllowAnonymous]
internal sealed class Index(IIdentityServerInteractionService interaction, IHostEnvironment environment) : PageModel
{
    public ErrorMessage? Message { get; private set; }

    public async Task OnGetAsync(string? errorId, CancellationToken ct)
    {
        ErrorMessage? message = await interaction.GetErrorContextAsync(errorId, ct);

        if (message is null)
        {
            return;
        }

        if (!environment.IsDevelopment())
        {
            // The description can leak configuration details, so only show it in development.
            message.ErrorDescription = null;
        }

        Message = message;
    }
}
