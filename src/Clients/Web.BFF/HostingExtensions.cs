using Duende.Bff;
using Duende.Bff.DynamicFrontends;
using Duende.Bff.EntityFramework;
using Duende.Bff.Yarp;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using Richie.ServiceDefaults;

using Web.BFF.Database;

namespace Web.BFF;

internal static class HostingExtensions
{
    private const string SessionsDbConnectionName = "web-bff-db";

    private const string MigrationsHistoryTable = "__ef_migrations_history";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        RequiredConfiguration required = new(builder.Configuration);

        string sessionsDbConnectionString = required.ConnectionString(SessionsDbConnectionName);
        string authority = required.HttpsAddress("Oidc:Authority");
        string clientId = required.Value("Oidc:ClientId");
        string clientSecret = required.Value("Oidc:ClientSecret");
        BffFrontend frontend = new(BffFrontendName.Parse("web-ui"));

        if (builder.Environment.IsDevelopment())
        {
            string devServerUrl = required.HttpsAddress("Frontend:DevServerUrl");

            if (devServerUrl.Length > 0)
            {
                frontend = frontend.WithProxiedStaticAssets(new Uri(devServerUrl));
            }
        }

        builder.Services.AddBff()
            .AddRemoteApis()
            .ConfigureOpenIdConnect(options =>
            {
                options.Authority = authority;

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                options.ResponseType = "code";
                options.ResponseMode = "query";

                options.GetClaimsFromUserInfoEndpoint = true;
                options.MapInboundClaims = false;
                options.SaveTokens = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
            })
            .AddFrontends(frontend)
            .AddEntityFrameworkServerSideSessions(db =>
            {
                db
                    .UseNpgsql(sessionsDbConnectionString, npgsql =>
                    {
                        npgsql.MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name);
                        npgsql.MigrationsHistoryTable(MigrationsHistoryTable, Schemas.Sessions);
                    })
                    .UseSnakeCaseNamingConvention();
            })
            .ConfigureEntityFrameworkSessionStoreOptions(static options =>
            {
                options.DefaultSchema = Schemas.Sessions;
                options.UserSessions.Name = "user_sessions";
            })
            .AddSessionCleanupBackgroundProcess();

        builder.EnrichNpgsqlDbContext<SessionDbContext>();

        builder.Services.AddAuthorization();

        builder.Services.AddDataProtection()
            .SetApplicationName("Web.BFF");

        WebApplication app = builder.Build();

        required.ThrowIfInvalid();

        return app;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseRouting();
        app.UseBff();
        app.UseAuthorization();

        if (!app.Environment.IsDevelopment())
        {
            app.MapFallbackToFile("index.html");
        }

        return app;
    }
}
