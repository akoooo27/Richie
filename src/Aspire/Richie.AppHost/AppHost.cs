IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithHttpHealthCheck("/health");

await builder
    .Build()
    .RunAsync();
