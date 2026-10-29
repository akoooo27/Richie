using Duende.Bff;
using Duende.Bff.Builder;
using Duende.Bff.EntityFramework;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

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

        builder.Services
            .AddBff(options =>
            {
                options.SessionCleanupInterval = TimeSpan.FromMinutes(5);
            })
            .AddServerSideSessions()
            .AddEntityFrameworkServerSideSessionsServices<SessionDbContext, IBffServicesBuilder>()
            .ConfigureEntityFrameworkSessionStoreOptions(options =>
            {
                options.DefaultSchema = Schemas.Bff;

                options.UserSessions = new TableConfiguration("user_sessions");
            })
            .AddSessionCleanupBackgroundProcess()
            .ConfigureCookies(options =>
            {
                options.Cookie.Name = "__Host-bff";
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

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
        app.UseAuthentication();
        app.UseBff();
        app.UseAuthorization();
        app.MapBffManagementEndpoints();

        return app;
    }
}
