using Serilog;
using Serilog.Events;

namespace Axpo.PowerTradeForecast.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging()
        {
         Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug() // Set the minimum level of logs to capture
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Override for specific namespaces
        .Enrich.FromLogContext() // Enrich logs with contextual information
        .WriteTo.Console(
           outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        )
        .WriteTo.File(
           "logs/log" + DateTime.Today + ".txt",
           rollingInterval: RollingInterval.Day,
           retainedFileCountLimit: 10, // Retain logs for the last 7 days
           outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        )
        .CreateLogger();
        }
    }
}