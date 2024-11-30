using Axpo.PowerTradeForecast.Application.DTOs;
using FluentAssertions;
using PowerTradeApp.Application.Validators;
using Xunit;

namespace PowerTradeApp.Tests
{
    public class HourlyVolumeValidatorTests
    {
        [Fact]
        public void Validate_ShouldSucceed_WhenAllFieldsAreValid()
        {
            var validator = new HourlyVolumeValidator();
            var hourlyVolume = new HourlyVolume { Datetime = DateTime.UtcNow.ToString(), Volume = 100 };

            var result = validator.Validate(hourlyVolume);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldFail_WhenDatetimeIsNull()
        {
            var validator = new HourlyVolumeValidator();
            var hourlyVolume = new HourlyVolume { Datetime = null, Volume = 100 };

            var result = validator.Validate(hourlyVolume);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Datetime" && e.ErrorMessage == "Datetime cannot be null.");
        }

        [Fact]
        public void Validate_ShouldFail_WhenVolumeIsZero()
        {
            var validator = new HourlyVolumeValidator();
            var hourlyVolume = new HourlyVolume { Datetime = DateTime.UtcNow.ToString(), Volume = -1 };

            var result = validator.Validate(hourlyVolume);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Volume" && e.ErrorMessage == "Volume must be greater than zero.");
        }
    }
}
