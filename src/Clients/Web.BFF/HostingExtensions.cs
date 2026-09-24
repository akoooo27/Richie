using Microsoft.AspNetCore.DataProtection;

using Richie.ServiceDefaults;

namespace Web.BFF;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        return app;
    }
}
