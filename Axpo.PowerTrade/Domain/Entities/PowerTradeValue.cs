namespace Axpo.PowerTradeForecast.Domain.Entities
{
    public class PowerTradeValue
    {
        public string TradeId { get; set; }
        public DateTime Date { get; set; }
        public List<PowerPeriod> Periods { get; set; }
    }
}