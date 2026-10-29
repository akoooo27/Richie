using Duende.Bff;
using Duende.Bff.Builder;
using Duende.Bff.DynamicFrontends;
using Duende.Bff.EntityFramework;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using Richie.ServiceDefaults;

using Web.BFF.Database;

namespace Web.BFF;

internal static class HostingExtensions
{
    private const string SessionDbConnectionName = "web-bff-db";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddNpgsqlDbContext<SessionDbContext>
        (
            SessionDbConnectionName,
            configureDbContextOptions: options => options
                .UseSnakeCaseNamingConvention()
                .UseNpgsql(npgsql => npgsql
                    .MigrationsHistoryTable("__ef_migrations_history", Schemas.Bff)
                    .MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name)
                )
        );

        string authority = builder.Configuration["Authentication:Oidc:Authority"] ??
                           throw new InvalidOperationException("OIDC authority is not configured.");

        string clientId = builder.Configuration["Authentication:Oidc:ClientId"] ??
                          throw new InvalidOperationException("OIDC client ID is not configured.");

        string clientSecret = builder.Configuration["Authentication:Oidc:ClientSecret"] ??
                              throw new InvalidOperationException("OIDC client secret is not configured.");

        Uri spaDevServerUrl = builder.Configuration.GetValue<Uri>("Spa:DevServerUrl") ??
                              throw new InvalidOperationException("SPA dev server URL is not configured.");

        builder.Services
            .AddBff(options =>
            {
                options.SessionCleanupInterval = TimeSpan.FromMinutes(5);
            })
            .ConfigureOpenIdConnect(options =>
            {
                options.Authority = authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;

                options.SaveTokens = true;

                options.GetClaimsFromUserInfoEndpoint = true;
                options.MapInboundClaims = false;
                options.DisableTelemetry = true;

                options.Scope.Add(OpenIdConnectScope.OfflineAccess);
            })
            .AddFrontends
            (
                new BffFrontend(BffFrontendName.Parse("spa"))
                    .WithProxiedStaticAssets(spaDevServerUrl)
            )
            .AddServerSideSessions()
            .AddEntityFrameworkServerSideSessionsServices<SessionDbContext, IBffServicesBuilder>()
            .ConfigureEntityFrameworkSessionStoreOptions(options =>
            {
                options.DefaultSchema = Schemas.Bff;

                options.UserSessions = new TableConfiguration("user_sessions");
            })
            .AddSessionCleanupBackgroundProcess();

        builder.Services.AddAuthorization();

        builder.Services
            .AddDataProtection()
            .SetApplicationName("Web.BFF");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseBff();
        app.UseAuthorization();

        return app;
    }
}
