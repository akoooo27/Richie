using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .WithHttpHealthCheck("/health");

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

identityApi.WaitForCompletion(identityMigrations);
identityApi.WaitForCompletion(operationalMigrations);

IResourceBuilder<ProjectResource> webBff = builder.AddProject<Projects.Web_BFF>("web-bff")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .WithHttpHealthCheck("/health");

IResourceBuilder<EFMigrationResource> sessionMigrations = webBff
    .AddEFMigrations("bff-session-migrations", "Duende.Bff.EntityFramework.SessionDbContext")
    .WithMigrationOutputDirectory("Database/Migrations/Sessions")
    .WithReference(webBffDb)
    .WaitFor(webBffDb)
    .RunDatabaseUpdateOnStart();

webBff.WaitForCompletion(sessionMigrations);

await builder
    .Build()
    .RunAsync();
