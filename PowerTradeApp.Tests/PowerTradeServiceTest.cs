using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Axpo.PowerTradeForecast.Application.DTOs;
using Axpo.PowerTradeForecast.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Axpo.PowerTradeForecast.Application.Services;
using Axpo;

public class PowerTradeServiceTests
{
    private readonly Mock<IPowerService> _powerServiceMock;
    private readonly Mock<ILogger<ScheduledTask>> _loggerMock;

    public PowerTradeServiceTests()
    {
        _powerServiceMock = new Mock<IPowerService>();
        _loggerMock = new Mock<ILogger<ScheduledTask>>();

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