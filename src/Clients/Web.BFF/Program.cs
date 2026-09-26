using Web.BFF;

WebApplication app = WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigurePipeline();

await app.RunAsync();
