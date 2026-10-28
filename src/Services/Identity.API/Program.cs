using Identity.API;
using Identity.API.Storage;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder
    .ConfigureServices()
    .ConfigurePipeline();

if (app.Environment.IsDevelopment())
{
    await app.MigrateStorageAsync();
}

await app.RunAsync();
