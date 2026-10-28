IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureKeyVault("key-vault");

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("identity-db")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

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
    .WaitFor(identityDb);

await builder
    .Build()
    .RunAsync();
