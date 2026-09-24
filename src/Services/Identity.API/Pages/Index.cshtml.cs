using Duende.IdentityServer.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages;

[AllowAnonymous]
internal sealed class Index : PageModel
{
    public string? DisplayName
        => User.Identity?.IsAuthenticated == true
            ? User.GetDisplayName()
            : null;
}
