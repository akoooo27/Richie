using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

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

builder.AddViteApp("web-ui", "../../Clients/Web.UI")
    .WithBun()
    .WithExternalHttpEndpoints();

await builder
    .Build()
    .RunAsync();
