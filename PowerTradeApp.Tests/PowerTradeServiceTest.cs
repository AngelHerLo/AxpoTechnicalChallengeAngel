using Axpo;
using Axpo.PowerTradeForecast.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

public class PowerTradeServiceTests
{
    private readonly Mock<IPowerService> _powerServiceMock;
    private readonly Mock<ILogger<PowerTradeReportScheduler>> _loggerMock;

    public PowerTradeServiceTests()
    {
        _powerServiceMock = new Mock<IPowerService>();
        _loggerMock = new Mock<ILogger<PowerTradeReportScheduler>>();
    }

    [Fact]
    public async Task GetAggregatedTradesAsync_ShouldReturnAggregatedHourlyVolumes()
    {
        // Arrange

        var date = DateTime.UtcNow;
        var timeZone = TimeZoneInfo.Utc;

        var mockTrade = PowerTrade.Create(date, 1);

        _powerServiceMock.Setup(s => s.GetTradesAsync(It.IsAny<DateTime>()))
                        .ReturnsAsync(new List<PowerTrade> { mockTrade });

        var powerTradeService = new PowerTradeService(_powerServiceMock.Object, _loggerMock.Object);

        // Act
        var result = await powerTradeService.GetAggregatedTradesAsync(date, timeZone);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}