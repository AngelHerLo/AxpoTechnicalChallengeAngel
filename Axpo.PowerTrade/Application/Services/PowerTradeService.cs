using Axpo.PowerTradeForecast.Application.DTOs;
using Axpo.PowerTradeForecast.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;

namespace Axpo.PowerTradeForecast.Application.Services
{
    public class PowerTradeService : IPowerTradeService
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<PowerTradeReportScheduler> _logger;

        public PowerTradeService(IPowerService powerService, ILogger<PowerTradeReportScheduler> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }

        public async Task<IEnumerable<HourlyVolume>> GetAggregatedTradesAsync(DateTime date, TimeZoneInfo timeZone)
        {
            return await Policy.Handle<Exception>()
            .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5),
                    onRetry: (exception, retryAttempt, context) =>
                    {
                        _logger.LogWarning(
                            "Retry {RetryAttempt} due to {ExceptionMessage}. Next retry in {RetryDelay}s.",
                            retryAttempt,
                            exception.Message,
                            5);
                    })
            .ExecuteAsync(async () =>
            {
                // Fetch day-ahead trades
                var trades = await _powerService.GetTradesAsync(date.AddDays(1));

                // Aggregate hourly volumes
                var hourlyVolumes = trades
                   .SelectMany(t => t.Periods)
                   .GroupBy(p => ConvertToIsoFormat(p.Period, timeZone))
                   .Select(g => new HourlyVolume
                   {
                       Datetime = g.Key,
                       Volume = Math.Round(g.Sum(p => p.Volume), 2)
                   });

                foreach (var trade in trades)
                {
                    _logger.LogInformation($"The Trade {trade.TradeId} has been succesfuly processed");
                }

                return hourlyVolumes;
            });            
        }

        private string ConvertToIsoFormat(int period, TimeZoneInfo timeZone)
        {
            DateTime utcNow = DateTime.UtcNow;

            // Set the time to UTC and conver to ISO
            String utcTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0, DateTimeKind.Utc).AddHours(period - 1).ToString("o");
            return utcTime;
        }
    }
}