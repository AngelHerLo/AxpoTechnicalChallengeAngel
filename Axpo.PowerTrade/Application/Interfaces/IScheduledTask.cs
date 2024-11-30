namespace Axpo.PowerTradeForecast.Application.Interfaces
{
    public interface IScheduledTask
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}