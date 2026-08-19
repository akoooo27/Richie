using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

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

identityApi.WaitForCompletion(identityMigrations);

await builder
    .Build()
    .RunAsync();
