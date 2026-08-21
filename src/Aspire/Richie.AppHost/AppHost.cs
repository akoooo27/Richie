using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<ParameterResource> bffClientSecret = builder.AddParameter
(
    name: "bff-client-secret",
    value: new GenerateParameterDefault { MinLength = 32, Special = false },
    secret: true,
    persist: true
);

IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .WithHttpHealthCheck("/health");

IResourceBuilder<ProjectResource> webBff = builder.AddProject<Projects.Web_BFF>("web-bff")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .WithHttpHealthCheck("/health")
    .WithReference(identityApi)
    .WaitFor(identityApi)
    .WithEnvironment("Oidc__Authority", identityApi.GetEndpoint("https"))
    .WithEnvironment("Oidc__ClientSecret", bffClientSecret);

identityApi
    .WithEnvironment("BffClient__BaseUrl", webBff.GetEndpoint("https"))
    .WithEnvironment("BffClient__Secret", bffClientSecret);

IResourceBuilder<EFMigrationResource> identityMigrations = identityApi
    .AddEFMigrations("identity-migrations", "Identity.API.Database.ApplicationDbContext")
    .WithMigrationOutputDirectory("Database/Migrations/Identity")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .RunDatabaseUpdateOnStart();

IResourceBuilder<EFMigrationResource> operationalMigrations = identityApi
    .AddEFMigrations("operational-migrations", "Duende.IdentityServer.EntityFramework.DbContexts.PersistedGrantDbContext")
    .WithMigrationOutputDirectory("Database/Migrations/Operational")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .RunDatabaseUpdateOnStart();

IResourceBuilder<EFMigrationResource> sessionMigrations = webBff
    .AddEFMigrations("bff-session-migrations", "Duende.Bff.EntityFramework.SessionDbContext")
    .WithMigrationOutputDirectory("Database/Migrations/Sessions")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .RunDatabaseUpdateOnStart();

identityApi.WaitForCompletion(identityMigrations);
identityApi.WaitForCompletion(operationalMigrations);
webBff.WaitForCompletion(sessionMigrations);

await builder
    .Build()
    .RunAsync();
