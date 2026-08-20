using System.Globalization;

using Identity.API.Database;
using Identity.API.Database.Entities;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        builder.AddNpgsqlDbContext<ApplicationDbContext>
        (
            connectionName: "identity-db",
            configureDbContextOptions: static options => options
                .UseNpgsql(static npgsql =>
                    npgsql.MigrationsHistoryTable("__ef_migrations_history", Schemas.Identity))
                .UseSnakeCaseNamingConvention()
        );

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer()
            .AddOperationalStore(options =>
            {
                // Eager zone: assignments only — this body runs at design time (verified).
                options.DefaultSchema = Schemas.Operational;

                options.DeviceFlowCodes.Name = "device_flow_codes";
                options.Keys.Name = "keys";
                options.PersistedGrants.Name = "persisted_grants";
                options.PushedAuthorizationRequests.Name = "par";
                options.SamlLogoutSessionRequestIndices.Name = "saml_logout_session_request_indices";
                options.SamlLogoutSessions.Name = "saml_logout_sessions";
                options.SamlSigninStates.Name = "saml_signin_states";
                options.ServerSideSessions.Name = "server_side_sessions";

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
                options.TokenCleanupBatchSize = 100;

                // Deferred zone — but it ALSO runs on every dotnet ef command (verified),
                // so the connection string read must stay null-tolerant.
                options.ResolveDbContextOptions = static (provider, dbOptions) =>
                {
                    string? connectionString = provider
                        .GetRequiredService<IConfiguration>()
                        .GetConnectionString("identity-db");

                    dbOptions.UseNpgsql(connectionString, static npgSql =>
                    {
                        npgSql.MigrationsAssembly("Identity.API");
                        npgSql.MigrationsHistoryTable("__ef_migrations_history", Schemas.Operational);
                    });

                    dbOptions.UseSnakeCaseNamingConvention();
                };
            })
            .AddServerSideSessions()
            .AddAspNetIdentity<ApplicationUser>();

        builder.Services.AddAuthorization();

        builder.Services.AddDataProtection()
            .SetApplicationName("Identity.API");

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

        app.UseIdentityServer();
        app.UseAuthorization();

        return app;
    }

    private static bool IsHealthProbe(PathString path) =>
        path.StartsWithSegments(Extensions.HealthEndpointPath, StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments(Extensions.AlivenessEndpointPath, StringComparison.OrdinalIgnoreCase);
}
