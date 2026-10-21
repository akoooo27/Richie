using Aspire.Hosting.Azure;
using Aspire.Hosting.EntityFrameworkCore;
using Aspire.Hosting.JavaScript;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<AzureKeyVaultResource> keyVault = builder.AddAzureKeyVault("key-vault");

IResourceBuilder<ParameterResource> webBffClientSecret = builder.AddParameter
(
    name: "web-bff-client-secret",
    value: new GenerateParameterDefault { MinLength = 32 },
    secret: true,
    persist: true
);

IResourceBuilder<ProjectResource> identityApi = builder
    .AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WithReference(keyVault)
    .WaitFor(identityDb);

IResourceBuilder<ViteAppResource> web = builder.AddViteApp("web", "../../Clients/web");

IResourceBuilder<ProjectResource> webBff = builder
    .AddProject<Projects.Web_BFF>("web-bff")
    .WithReference(webBffDb)
    .WithReference(identityApi)
    .WithReference(web.GetEndpoint("http"))
    .WithEnvironment("Oidc__ClientSecret", webBffClientSecret)
    .WithExternalHttpEndpoints()
    .WaitFor(webBffDb)
    .WaitFor(web);

identityApi
    .WithEnvironment("Clients__WebBff__BaseUrl", webBff.GetEndpoint("https"))
    .WithEnvironment("Clients__WebBff__Secret", webBffClientSecret);

IResourceBuilder<EFMigrationResource> identityOperationalMigrations = identityApi
    .AddEFMigrations
    (
        name: "identity-operational-migrations",
        dbContextTypeName: "Duende.IdentityServer.EntityFramework.DbContexts.PersistedGrantDbContext",
        configureToolResource: tool => tool.WithToolVersion("10.0.13")
    )
    .WithMigrationOutputDirectory("Database/Migrations/Operational")
    .WaitFor(identityDb)
    .RunDatabaseUpdateOnStart();

identityApi.WaitForCompletion(identityOperationalMigrations);

IResourceBuilder<EFMigrationResource> webBffSessionMigrations = webBff
    .AddEFMigrations
    (
        name: "web-bff-session-migrations",
        dbContextTypeName: "Duende.Bff.EntityFramework.SessionDbContext",
        configureToolResource: tool => tool.WithToolVersion("10.0.13")
    )
    .WithMigrationOutputDirectory("Database/Migrations/Sessions")
    .WaitFor(webBffDb)
    .RunDatabaseUpdateOnStart();

webBff.WaitForCompletion(webBffSessionMigrations);

await builder
    .Build()
    .RunAsync();
