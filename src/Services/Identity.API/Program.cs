using System.Globalization;

using Identity.API;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up Identity.API");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    WebApplication app = builder
        .ConfigureLogging()
        .ConfigureServices()
        .Build()
        .ConfigurePipeline();

    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Identity.API terminated unexpectedly");

    return 1;
}
finally
{
    Log.Information("Shut down Identity.API complete");
    await Log.CloseAndFlushAsync();
}

return 0;
