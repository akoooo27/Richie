IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

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
