using Axpo.PowerTradeForecast.Application.DTOs;
using Axpo.PowerTradeForecast.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Serilog;
using Serilog.Core;

namespace Axpo.PowerTradeForecast.Application.Services
{
    public class PowerTradeService : IPowerTradeService
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<ScheduledTask> _logger;

        public PowerTradeService(IPowerService powerService, ILogger<ScheduledTask> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }

        public async Task<IEnumerable<HourlyVolume>> GetAggregatedTradesAsync(DateTime date, TimeZoneInfo timeZone)
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
                    Volume = Math.Round(g.Sum(p => p.Volume),2)
                });

            foreach (var trade in trades)
            {
                _logger.LogInformation($"The Trade {trade.TradeId} has been succesefully processed");
            }

            return hourlyVolumes;
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