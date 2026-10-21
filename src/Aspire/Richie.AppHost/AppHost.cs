using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<ProjectResource> identityApi = builder
    .AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WaitFor(identityDb);

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

await builder
    .Build()
    .RunAsync();
