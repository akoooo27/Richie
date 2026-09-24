using Duende.IdentityServer.EntityFramework.DbContexts;

using Identity.API.Database;
using Identity.API.Database.Entities;
using Identity.API.Pages;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Richie.ServiceDefaults;

namespace Identity.API;

internal static class HostingExtensions
{
    private const string IdentityDbConnectionName = "identity-db";

    private const string MigrationsHistoryTable = "__ef_migrations_history";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddRazorPages(static options =>
        {
            options.Conventions.ConfigureFilter(new SecurityHeadersAttribute());
        });

        string? identityDbConnectionString = builder.Configuration.GetConnectionString(IdentityDbConnectionName);

        if (identityDbConnectionString is null && !EF.IsDesignTime)
        {
            throw new InvalidOperationException($"Connection string '{IdentityDbConnectionName}' is missing.");
        }

        builder.AddNpgsqlDbContext<ApplicationDbContext>
        (
            connectionName: IdentityDbConnectionName,
            configureDbContextOptions: static options =>
            {
                options
                    .UseNpgsql(static npgsql =>
                    {
                        npgsql.MigrationsHistoryTable(MigrationsHistoryTable, Schemas.Identity);
                    })
                    .UseSnakeCaseNamingConvention();
            }
        );

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryClients([])
            .AddInMemoryIdentityResources([])
            .AddInMemoryApiScopes([])
            .AddOperationalStore(options =>
            {
                options.DefaultSchema = Schemas.Operational;

                options.ConfigureDbContext = db =>
                {
                    db
                        .UseNpgsql(identityDbConnectionString, npgSql =>
                        {
                            npgSql.MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name);
                            npgSql.MigrationsHistoryTable(MigrationsHistoryTable, Schemas.Operational);
                        })
                        .UseSnakeCaseNamingConvention();
                };

                options.PersistedGrants.Name = "persisted_grants";
                options.DeviceFlowCodes.Name = "device_codes";
                options.Keys.Name = "keys";
                options.ServerSideSessions.Name = "server_side_sessions";
                options.PushedAuthorizationRequests.Name = "pushed_authorization_requests";
                options.SamlSigninStates.Name = "saml_signin_states";
                options.SamlLogoutSessions.Name = "saml_logout_sessions";
                options.SamlLogoutSessionRequestIndices.Name = "saml_logout_session_request_indices";

                options.EnableTokenCleanup = true;
                options.RemoveConsumedTokens = true;
            })
            .AddAspNetIdentity<ApplicationUser>();

        builder.EnrichNpgsqlDbContext<PersistedGrantDbContext>();

        builder.Services.AddAuthorization();

        builder.Services.AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}
