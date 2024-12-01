using Axpo;
using Axpo.PowerTradeForecast.Application.Interfaces;
using Axpo.PowerTradeForecast.Application.Services;
using Axpo.PowerTradeForecast.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(context.HostingEnvironment.ContentRootPath);
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));

        // Add services
        services.AddTransient<IPowerService, PowerService>();
        services.AddSingleton<PowerService>();
        services.AddScoped<IPowerTradeService, PowerTradeService>();
        services.AddScoped<ICsvWriterService, CsvWriterService>();
        services.AddHostedService<PowerTradeReportScheduler>();

        // Logging
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console()
            .WriteTo.File("logs/Log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    })
    .UseSerilog();

await builder.RunConsoleAsync();