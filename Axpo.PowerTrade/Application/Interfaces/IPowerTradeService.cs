using Axpo.PowerTradeForecast.Application.DTOs;

namespace Axpo.PowerTradeForecast.Application.Interfaces
{
    public interface IPowerTradeService
    {
        Task<IEnumerable<HourlyVolume>> GetAggregatedTradesAsync(DateTime date, TimeZoneInfo timeZone);
    }
}