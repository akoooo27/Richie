using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;

namespace Identity.API.Pages;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class SecurityHeadersAttribute : ActionFilterAttribute
{
    private const string ContentSecurityPolicy =
        "default-src 'self'; " +
        "img-src 'self' data:; " +
        "object-src 'none'; " +
        "frame-ancestors 'none'; " +
        "sandbox allow-forms allow-same-origin allow-scripts; " +
        "base-uri 'self';";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not PageResult)
        {
            return;
        }

        IHeaderDictionary headers = context.HttpContext.Response.Headers;

        headers.TryAdd(HeaderNames.XContentTypeOptions, "nosniff");
        headers.TryAdd(HeaderNames.XFrameOptions, "DENY");
        headers.TryAdd(HeaderNames.ContentSecurityPolicy, ContentSecurityPolicy);
        headers.TryAdd("Referrer-Policy", "no-referrer");
    }
}
