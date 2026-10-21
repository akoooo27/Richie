using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;

using Identity.API.Configuration;
using Identity.API.Database;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using Richie.ServiceDefaults;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        string? dbConnectionString = builder.Configuration.GetConnectionString("identity-db");

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddOperationalStore(options =>
            {
                options.DefaultSchema = Schemas.Operational;


                options.PersistedGrants = new TableConfiguration(OperationalTables.PersistedGrants);
                options.DeviceFlowCodes = new TableConfiguration(OperationalTables.DeviceCodes);
                options.Keys = new TableConfiguration(OperationalTables.Keys);
                options.ServerSideSessions = new TableConfiguration(OperationalTables.ServerSideSessions);
                options.PushedAuthorizationRequests = new TableConfiguration(OperationalTables.PushedAuthorizationRequests);
                options.SamlSigninStates = new TableConfiguration(OperationalTables.SamlSigninStates);
                options.SamlLogoutSessions = new TableConfiguration(OperationalTables.SamlLogoutSessions);
                options.SamlLogoutSessionRequestIndices = new TableConfiguration(OperationalTables.SamlLogoutSessionRequestIndices);

                options.ConfigureDbContext = dbContext =>
                {
                    dbContext
                        .UseSnakeCaseNamingConvention()
                        .UseNpgsql(dbConnectionString, npgSqlOptions =>
                        {
                            npgSqlOptions.MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name);
                            npgSqlOptions.MigrationsHistoryTable("__ef_migrations_history", Schemas.Operational);
                        });
                };

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
                options.TokenCleanupBatchSize = 100;
            })
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources());

        builder.EnrichNpgsqlDbContext<PersistedGrantDbContext>();

        builder.Services
            .AddDataProtection()
            .SetApplicationName("IdentityServer");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseIdentityServer();

        app.MapDefaultEndpoints();

        return app;
    }
}
