using Aspire.Hosting.EntityFrameworkCore;

const string webBffClientId = "web-bff";

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureKeyVault("key-vault");

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> userManagementDb = postgres.AddDatabase("user-management-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<ParameterResource> webBffClientSecret = builder.AddParameter
(
    name: "web-bff-client-secret",
    value: new GenerateParameterDefault { MinLength = 32 },
    secret: true,
    persist: true
);

builder
    .AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithDataVolume();

builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume();

IResourceBuilder<ProjectResource> identityApi = builder
    .AddProject<Projects.Identity_API>("identity-api", launchProfileName: "https")
    .WithReference(identityDb)
    .WithReference(userManagementDb)
    .WaitFor(identityDb)
    .WaitFor(userManagementDb)
    .WithEnvironment("Clients__WebBff__ClientId", webBffClientId)
    .WithEnvironment("Clients__WebBff__ClientSecret", webBffClientSecret);

IResourceBuilder<ProjectResource> webBff = builder
    .AddProject<Projects.Web_BFF>("web-bff", launchProfileName: "https")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .WithEnvironment("Authentication__Oidc__Authority", identityApi.GetEndpoint("https"))
    .WithEnvironment("Authentication__Oidc__ClientId", webBffClientId)
    .WithEnvironment("Authentication__Oidc__ClientSecret", webBffClientSecret);

identityApi.WithEnvironment("Clients__WebBff__BaseUrl", webBff.GetEndpoint("https"));

IResourceBuilder<EFMigrationResource> webBffMigrations = webBff
    .AddEFMigrations("web-bff-migrations", "Duende.Bff.EntityFramework.SessionDbContext")
    .WithMigrationOutputDirectory("Database/Migrations")
    .WithMigrationNamespace("Web.BFF.Database.Migrations")
    .RunDatabaseUpdateOnStart();

webBff.WaitForCompletion(webBffMigrations);

await builder
    .Build()
    .RunAsync();
