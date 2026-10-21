using Identity.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder
    .ConfigureServices()
    .ConfigurePipeline();

if (app.Environment.IsDevelopment())
{
    await app.MigrateUserManagementSchemaAsync();
}

await app.RunAsync();
