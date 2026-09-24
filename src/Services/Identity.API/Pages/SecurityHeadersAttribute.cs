using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class SecurityHeadersAttribute : ResultFilterAttribute
{
    private const string ContentSecurityPolicy =
        "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not PageResult)
        {
            return;
        }

        IHeaderDictionary headers = context.HttpContext.Response.Headers;

        headers.TryAdd("X-Content-Type-Options", "nosniff");
        headers.TryAdd("X-Frame-Options", "DENY");
        headers.TryAdd("Content-Security-Policy", ContentSecurityPolicy);
        headers.TryAdd("Referrer-Policy", "no-referrer");
    }
}
