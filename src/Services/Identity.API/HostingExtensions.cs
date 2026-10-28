using Microsoft.AspNetCore.DataProtection;

using Richie.ServiceDefaults;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        return app;
    }
}
