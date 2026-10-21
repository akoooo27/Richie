using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Duende.Storage.PostgreSql;
using Duende.Storage.Schema;
using Duende.UserManagement;

using Identity.API.Configuration;
using Identity.API.Database;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using Resend;

using Richie.ServiceDefaults;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Configuration.AddAzureKeyVaultSecrets("key-vault");

        string? dbConnectionString = builder.Configuration.GetConnectionString("identity-db");

        builder.AddNpgsqlDataSource("identity-db");

        builder.Services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddOperationalStore(options =>
            {
                options.DefaultSchema = Schemas.Operational;

                options.PersistedGrants = new TableConfiguration("persisted_grants");
                options.DeviceFlowCodes = new TableConfiguration("device_codes");
                options.Keys = new TableConfiguration("keys");
                options.ServerSideSessions = new TableConfiguration("server_side_sessions");
                options.PushedAuthorizationRequests = new TableConfiguration("pushed_authorization_requests");
                options.SamlSigninStates = new TableConfiguration("saml_signin_states");
                options.SamlLogoutSessions = new TableConfiguration("saml_logout_sessions");
                options.SamlLogoutSessionRequestIndices = new TableConfiguration("saml_logout_session_request_indices");

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
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
            .AddUserManagement(userManagement =>
            {
                userManagement.AddPostgreSqlStore(postgresOptions =>
                {
                    postgresOptions.SchemaName = Schemas.UserManagement;
                });
            });

        builder.EnrichNpgsqlDbContext<PersistedGrantDbContext>();

        builder.Services.AddResend(options =>
        {
            options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
        });

        builder.Services
            .AddDataProtection()
            .SetApplicationName("IdentityServer");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseIdentityServer();

        app.MapDefaultEndpoints();

        app.MapUserManagement();

        return app;
    }

    public static async Task MigrateUserManagementSchemaAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = app.Services.CreateScope();

        IDatabaseSchema schema = scope.ServiceProvider.GetRequiredService<IDatabaseSchema>();

        await schema.MigrateAsync(cancellationToken);
    }
}
