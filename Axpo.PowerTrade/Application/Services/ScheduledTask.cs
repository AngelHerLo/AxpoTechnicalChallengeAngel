using Axpo.PowerTradeForecast.Application.Interfaces;
using Axpo.PowerTradeForecast.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axpo.PowerTradeForecast.Application.Services
{
    public class ScheduledTask : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _settings;
        private readonly ILogger<ScheduledTask> _logger;

        public ScheduledTask(IServiceProvider serviceProvider, IOptions<AppSettings> settings, ILogger<ScheduledTask> logger)
        {
            _serviceProvider = serviceProvider;
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled task started.");

            do
            {
                using var scope = _serviceProvider.CreateScope();
                var powerTradeService = scope.ServiceProvider.GetRequiredService<IPowerTradeService>();
                var csvWriter = scope.ServiceProvider.GetRequiredService<ICsvWriterService>();

                try
                {
                    // Generate the data
                    var data = await powerTradeService.GetAggregatedTradesAsync(DateTime.Today,
                        TimeZoneInfo.FindSystemTimeZoneById(_settings.TimeZone));

                    // Define file naming
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmm");
                    var date = DateTime.Today.AddDays(1).ToString("yyyyMMdd");
                    var filePath = Path.Combine(_settings.OutputFolder, $"PowerPosition_{date}_{timestamp}.csv");

                    // Write CSV
                    csvWriter.WriteToCsv(filePath, data);
                    _logger.LogInformation($"CSV file created at: {filePath}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(_settings.IntervalMinutes), stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}
