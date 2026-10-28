IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureKeyVault("key-vault");

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> userManagementDb = postgres.AddDatabase("user-management-db");

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

await builder
    .Build()
    .RunAsync();
