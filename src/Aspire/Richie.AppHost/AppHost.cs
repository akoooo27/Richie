using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WaitFor(identityDb);

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

builder.AddViteApp("web-ui", "../../Clients/Web.UI")
    .WithBun()
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Web_BFF>("web-bff")
    .WithReference(webBffDb)
    .WaitFor(webBffDb);

await builder
    .Build()
    .RunAsync();
