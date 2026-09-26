using Aspire.Hosting.EntityFrameworkCore;
using Aspire.Hosting.JavaScript;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<ParameterResource> webBffClientSecret = builder.AddParameter
(
    "web-bff-client-secret",
    new GenerateParameterDefault { MinLength = 32 },
    secret: true,
    persist: true
);

const string webBffClientId = "web-bff";

IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.Identity_API>("identity-api", "https")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(identityDb)
    .WaitFor(identityDb);

#pragma warning disable ASPIRECERTIFICATES001
IResourceBuilder<ViteAppResource> webUi = builder.AddViteApp("web-ui", "../../Clients/Web.UI")
    .WithBun()
    .WithHttpsEndpoint(env: "PORT")
    .WithHttpsDeveloperCertificate();
#pragma warning restore ASPIRECERTIFICATES001

IResourceBuilder<ProjectResource> webBff = builder.AddProject<Projects.Web_BFF>("web-bff", "https")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .WithEnvironment("Oidc__Authority", identityApi.GetEndpoint("https"))
    .WithEnvironment("Oidc__ClientId", webBffClientId)
    .WithEnvironment("Oidc__ClientSecret", webBffClientSecret)
    .WaitFor(identityApi)
    .PublishWithContainerFiles(webUi, "./wwwroot");

if (builder.ExecutionContext.IsRunMode)
{
    webBff
        .WithEnvironment("Frontend__DevServerUrl", webUi.GetEndpoint("https"))
        .WaitFor(webUi);
}

identityApi
    .WithEnvironment("Clients__WebBff__BaseUrl", webBff.GetEndpoint("https"))
    .WithEnvironment("Clients__WebBff__ClientId", webBffClientId)
    .WithEnvironment("Clients__WebBff__ClientSecret", webBffClientSecret);

IResourceBuilder<EFMigrationResource> identityUsersMigrations = identityApi
    .AddEFMigrations
    (
        name: "identity-users-migrations",
        dbContextTypeName: "Identity.API.Database.ApplicationDbContext"
    )
    .WithMigrationOutputDirectory("Database/Migrations/ApplicationDb")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .RunDatabaseUpdateOnStart();

identityApi.WaitForCompletion(identityUsersMigrations);

IResourceBuilder<EFMigrationResource> identityOperationalMigrations = identityApi
    .AddEFMigrations
    (
        name: "identity-operational-migrations",
        dbContextTypeName: "Duende.IdentityServer.EntityFramework.DbContexts.PersistedGrantDbContext"
    )
    .WithMigrationOutputDirectory("Database/Migrations/PersistedGrantDb")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .RunDatabaseUpdateOnStart();

identityApi.WaitForCompletion(identityOperationalMigrations);

IResourceBuilder<EFMigrationResource> webBffSessionsMigrations = webBff
    .AddEFMigrations
    (
        name: "web-bff-sessions-migrations",
        dbContextTypeName: "Duende.Bff.EntityFramework.SessionDbContext"
    )
    .WithMigrationOutputDirectory("Database/Migrations/SessionDb")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .RunDatabaseUpdateOnStart();

webBff.WaitForCompletion(webBffSessionsMigrations);

await builder
    .Build()
    .RunAsync();
