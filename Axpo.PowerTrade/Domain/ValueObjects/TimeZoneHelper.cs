namespace Axpo.PowerTradeForecast.Domain.ValueObjects
{
    public static class TimeZoneHelper
    {
        public static DateTime ConvertToUtc(DateTime localDateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }
    }
}