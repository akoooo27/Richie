using Aspire.Hosting.EntityFrameworkCore;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureKeyVault("key-vault");

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> userManagementDb = postgres.AddDatabase("user-management-db");

IResourceBuilder<PostgresDatabaseResource> webBffDb = postgres.AddDatabase("web-bff-db");

builder
    .AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithDataVolume();

builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume();

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WithReference(userManagementDb)
    .WaitFor(identityDb)
    .WaitFor(userManagementDb);

IResourceBuilder<ProjectResource> webBff = builder.AddProject<Projects.Web_BFF>("web-bff")
    .WithReference(webBffDb)
    .WaitFor(webBffDb);

IResourceBuilder<EFMigrationResource> webBffMigrations = webBff
    .AddEFMigrations("web-bff-migrations", "Duende.Bff.EntityFramework.SessionDbContext")
    .WithMigrationOutputDirectory("Database/Migrations")
    .WithMigrationNamespace("Web.BFF.Database.Migrations")
    .RunDatabaseUpdateOnStart();

webBff.WaitForCompletion(webBffMigrations);

await builder
    .Build()
    .RunAsync();
