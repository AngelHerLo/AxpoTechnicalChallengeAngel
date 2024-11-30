namespace Axpo.PowerTradeForecast.Application.DTOs
{
    public class HourlyVolume
    {
        public string Datetime { get; set; } // In UTC and string to apply ISO 8601
        public double Volume { get; set; }
    }
}