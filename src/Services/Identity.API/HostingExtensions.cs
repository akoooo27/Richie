using Duende.IdentityServer;
using Duende.Storage.PostgreSql;
using Duende.UserManagement.Authentication.Otp;

using Identity.API.Authentication.Otp;
using Identity.API.Configuration;
using Identity.API.Storage;

using Microsoft.AspNetCore.DataProtection;

using Npgsql;

using Resend;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Configuration.AddAzureKeyVaultSecrets("key-vault");

        builder.AddNpgsqlDataSource(IdentityDatabase.ConnectionStringName);
        builder.AddKeyedNpgsqlDataSource(IdentityUsersDatabase.ConnectionStringName);

        builder.Services.AddScoped<IOtpDispatcher, ResendOtpDispatcher>();

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

        string apiToken = builder.Configuration["Resend:ApiToken"]
            ?? throw new InvalidOperationException("Configuration value 'Resend:ApiToken' is required.");

        builder.Services
            .AddOptions<ResendOtpOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddResend(options =>
        {
            options.ApiToken = apiToken;
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        return app;
    }
}
