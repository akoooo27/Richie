using Identity.API;

WebApplication app = WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigurePipeline();

await app.RunAsync();
