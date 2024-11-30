using FluentAssertions;
using Axpo.PowerTradeForecast.Application.DTOs;
using Axpo.PowerTradeForecast.Application.Services;

namespace PowerTradeApp.Tests
{
    public class CsvWriterServiceTests
    {
        [Fact]
        public void WriteToCsv_ShouldGenerateCsvFile_WithValidData()
        {
            // Arrange
            var service = new CsvWriterService();
            var hourlyVolumes = new List<HourlyVolume>
        {
            new HourlyVolume { Datetime = DateTime.UtcNow.ToString(), Volume = 100 }
        };
            var filePath = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            service.WriteToCsv(filePath, hourlyVolumes);

            // Assert
            File.Exists(filePath).Should().BeTrue();

            // Cleanup
            File.Delete(filePath);
        }
    }
}