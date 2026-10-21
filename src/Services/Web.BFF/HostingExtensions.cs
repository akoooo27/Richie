using Duende.Bff;
using Duende.Bff.EntityFramework;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using Richie.ServiceDefaults;

using Web.BFF.Database;

namespace Web.BFF;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        string? dbConnectionString = builder.Configuration.GetConnectionString("web-bff-db");

        builder.Services
            .AddBff(options =>
            {
                options.SessionCleanupInterval = TimeSpan.FromMinutes(10);
            })
            .ConfigureCookies(options =>
            {
                options.Cookie.Name = "__Host-bff";
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddEntityFrameworkServerSideSessions(options =>
            {
                options
                    .UseSnakeCaseNamingConvention()
                    .UseNpgsql(dbConnectionString, npgSqlOptions =>
                    {
                        npgSqlOptions.MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name);
                        npgSqlOptions.MigrationsHistoryTable("__ef_migrations_history", Schemas.Bff);
                    });
            })
            .ConfigureEntityFrameworkSessionStoreOptions(options =>
            {
                options.DefaultSchema = Schemas.Bff;
                options.UserSessions = new TableConfiguration("user_sessions");
            })
            .AddSessionCleanupBackgroundProcess();

        builder.EnrichNpgsqlDbContext<SessionDbContext>();

        builder.Services.AddAuthorization();

        builder.Services
            .AddDataProtection()
            .SetApplicationName("Web.BFF");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseRouting();

        app.UseAuthentication();

        app.UseBff();

        app.UseAuthorization();

        app.MapDefaultEndpoints();

        return app;
    }
}
