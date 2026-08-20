using System.Globalization;

using Serilog;

using Web.BFF;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up Web.BFF");

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
    Log.Fatal(ex, "Web.BFF terminated unexpectedly");

    return 1;
}
finally
{
    Log.Information("Shut down Web.BFF complete");
    await Log.CloseAndFlushAsync();
}

return 0;
