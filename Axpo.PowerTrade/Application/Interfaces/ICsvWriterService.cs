using Axpo.PowerTradeForecast.Application.DTOs;

namespace Axpo.PowerTradeForecast.Application.Interfaces
{
    public interface ICsvWriterService
    {
        void WriteToCsv(string filePath, IEnumerable<HourlyVolume> data);
    }
}