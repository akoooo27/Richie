IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Identity_API>("identity-api");

builder.AddViteApp("web-ui", "../../Clients/Web.UI")
    .WithBun()
    .WithExternalHttpEndpoints();

await builder
    .Build()
    .RunAsync();
