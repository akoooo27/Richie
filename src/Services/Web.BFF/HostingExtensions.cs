using System.Globalization;

using Duende.Bff;
using Duende.Bff.Builder;
using Duende.Bff.EntityFramework;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using Richie.ServiceDefaults;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

using Web.BFF.Configuration;
using Web.BFF.Database;

namespace Web.BFF;

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

        builder.AddNpgsqlDbContext<SessionDbContext>
        (
            connectionName: "web-bff-db",
            configureDbContextOptions: static options => options
                .UseNpgsql(static npgSql =>
                {
                    npgSql.MigrationsAssembly("Web.BFF");
                    npgSql.MigrationsHistoryTable("__ef_migrations_history", Schemas.Sessions);
                })
                .UseSnakeCaseNamingConvention()
        );

        builder.Services.AddOptions<OidcSettings>()
            .BindConfiguration(OidcSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddBff()
            .ConfigureOpenIdConnect(options =>
            {
                // Deferred zone: BFF stores this action in BffOptions and applies it when
                // the OIDC handler is materialized — never at design time.
                options.Authority = builder.Configuration["Oidc:Authority"];
                options.ClientId = builder.Configuration["Oidc:ClientId"];
                options.ClientSecret = builder.Configuration["Oidc:ClientSecret"];

                options.ResponseType = "code";
                options.ResponseMode = "query";

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.MapInboundClaims = false;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
            })
            .ConfigureCookies(static options => options.Cookie.SameSite = SameSiteMode.Strict)
            .AddEntityFrameworkServerSideSessionsServices<SessionDbContext, IBffServicesBuilder>()
            .ConfigureEntityFrameworkSessionStoreOptions(static options =>
            {
                options.DefaultSchema = Schemas.Sessions;

                options.UserSessions.Name = "user_sessions";
            })
            .AddSessionCleanupBackgroundProcess();

        builder.Services.AddAuthorization();

        builder.Services.AddDataProtection()
            .SetApplicationName("Web.BFF");

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

        app.UseRouting();
        app.UseAuthentication();
        app.UseBff();
        app.UseAuthorization();

        app.MapDefaultEndpoints();

        return app;
    }

    private static bool IsHealthProbe(PathString path) =>
        path.StartsWithSegments(Extensions.HealthEndpointPath, StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments(Extensions.AlivenessEndpointPath, StringComparison.OrdinalIgnoreCase);
}
