using Duende.IdentityServer;
using Duende.Storage.PostgreSql;
using Duende.UserManagement;

using Identity.API.Configuration;
using Identity.API.Storage;

using Microsoft.AspNetCore.DataProtection;

using Npgsql;

using Richie.ServiceDefaults;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddNpgsqlDataSource(StorageInstances.IdentityConnectionName);
        builder.AddKeyedNpgsqlDataSource(StorageInstances.UserManagementConnectionName);

        builder.Services.AddRazorPages();

        builder.Services
            .AddIdentityServer()
            .AddServerSideSessions()
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
            .AddInMemoryClients(IdentityServerConfiguration.GetClients(builder.Configuration))
            .AddStorage(storage => storage.AddPostgreSql(options => options.SchemaName = StorageInstances.SchemaName))
            .AddOperationalStorage()
            .AddStorage
            (
                storageInstanceId: StorageInstances.UserManagement,
                configure: storage => storage.AddPostgreSql
                (
                    serviceProvider =>
                        serviceProvider.GetRequiredKeyedService<NpgsqlDataSource>(StorageInstances
                            .UserManagementConnectionName),
                    options => options.SchemaName = StorageInstances.SchemaName
                )
            )
            .AddUserManagement(StorageInstances.UserManagement, _ => { });

        builder.Services
            .AddDataProtection()
            .SetApplicationName("Identity.API");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapDefaultEndpoints();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapUserManagement();

        return app;
    }
}
