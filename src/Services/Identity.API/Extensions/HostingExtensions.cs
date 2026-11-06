using Duende.IdentityServer;
using Duende.Storage.PostgreSql;
using Duende.UserManagement.Authentication.Otp;

using FastEndpoints;
using FastEndpoints.OpenApi;

using Identity.API.Authentication.Otp;
using Identity.API.Configuration;
using Identity.API.Storage;

using Microsoft.AspNetCore.DataProtection;

using Npgsql;

using Resend;

using Scalar.AspNetCore;

namespace Identity.API.Extensions;

internal static class HostingExtensions
{
    private const string ApiTitle = "Richie Identity API";
    private const string ApiV1 = "v1";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Configuration.AddAzureKeyVaultSecrets("key-vault");

        builder.AddNpgsqlDataSource(IdentityDatabase.ConnectionStringName);
        builder.AddKeyedNpgsqlDataSource(IdentityUsersDatabase.ConnectionStringName);

        builder.Services.AddScoped<IOtpDispatcher, ResendOtpDispatcher>();

        builder.Services.AddSingleton<OtpCookie>();

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
            .BindConfiguration(ResendOtpOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddResend(options =>
        {
            options.ApiToken = apiToken;
        });

        builder.Services.AddFastEndpoints(DiscoveredTypes.All);

        builder.Services.OpenApiDocument(options =>
        {
            options.DocumentName = ApiV1;
            options.Title = ApiTitle;
            options.Version = ApiV1;
            options.MaxEndpointVersion = 1;
            options.ExcludeNonFastEndpoints = true;
            options.ShortSchemaNames = true;
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options => options
                .WithTitle(ApiTitle)
                .AddDocument(ApiV1));
        }

        app.MapDefaultEndpoints();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseFastEndpoints(config =>
        {
            config.Binding.ReflectionCache.AddFromIdentityAPI();
            config.Endpoints.RoutePrefix = "api";
            config.Versioning.Prefix = "v";
            config.Versioning.DefaultVersion = 1;
            config.Versioning.PrependToRoute = true;
            config.Errors.UseProblemDetails();
        });

        return app;
    }
}
