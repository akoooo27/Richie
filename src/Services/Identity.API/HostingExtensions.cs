using Duende.IdentityServer;
using Duende.Storage.PostgreSql;

using Identity.API.Configuration;
using Identity.API.Storage;

using Microsoft.AspNetCore.DataProtection;

using Npgsql;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddNpgsqlDataSource(IdentityDatabase.ConnectionStringName);
        builder.AddKeyedNpgsqlDataSource(IdentityUsersDatabase.ConnectionStringName);

        builder.Services
            .AddIdentityServer()
            .AddStorage(storage => storage.AddPostgreSql(options => options.SchemaName = IdentityDatabase.Schema))
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
            .AddOperationalStorage()
            .AddStorage(IdentityUsersDatabase.InstanceId,
                storage => storage.AddPostgreSql(
                    sp => sp.GetRequiredKeyedService<NpgsqlDataSource>(IdentityUsersDatabase.ConnectionStringName),
                    options => options.SchemaName = IdentityUsersDatabase.Schema))
            .AddUserManagement(IdentityUsersDatabase.InstanceId, _ => { });

        builder.Services
            .AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        return app;
    }
}
