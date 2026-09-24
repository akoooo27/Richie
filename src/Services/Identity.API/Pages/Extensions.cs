using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages;

internal static class Extensions
{
    public static bool IsNativeClient(this AuthorizationRequest context)
    {
        return !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
    }

    public static IActionResult LoadingPage(this PageModel page, string? redirectUri)
    {
        return page.RedirectToPage("/Redirect/Index", new { RedirectUri = redirectUri });
    }
}
