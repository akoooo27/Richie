using Identity.API.Database;
using Identity.API.Database.Entities;

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

        builder.Services.AddAuthorization();

        builder.Services.AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        return app;
    }

}
