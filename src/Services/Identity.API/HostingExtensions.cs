using System.Globalization;

using Richie.ServiceDefaults;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog
        (
            configureLogger: (services, lc) =>
            {
                lc.ReadFrom.Configuration(builder.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console
                    (
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                        formatProvider: CultureInfo.InvariantCulture,
                        theme: AnsiConsoleTheme.Literate
                    );
            },
            writeToProviders: true
        );

        return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.AddServiceDefaults();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = static (httpContext, _, ex) =>
            {
                if (ex is not null || httpContext.Response.StatusCode >= 500)
                {
                    return LogEventLevel.Error;
                }

                return IsHealthProbe(httpContext.Request.Path)
                    ? LogEventLevel.Verbose
                    : LogEventLevel.Information;
            };
        });

        app.MapDefaultEndpoints();

        return app;
    }

    private static bool IsHealthProbe(PathString path) =>
        path.StartsWithSegments(Extensions.HealthEndpointPath, StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments(Extensions.AlivenessEndpointPath, StringComparison.OrdinalIgnoreCase);
}
