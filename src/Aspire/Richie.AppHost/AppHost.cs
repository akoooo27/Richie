IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureKeyVault("key-vault");

builder
    .AddPostgres("identity-db")
    .WithPgWeb()
    .WithDataVolume();

builder
    .AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithDataVolume();

builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume();

await builder
    .Build()
    .RunAsync();
