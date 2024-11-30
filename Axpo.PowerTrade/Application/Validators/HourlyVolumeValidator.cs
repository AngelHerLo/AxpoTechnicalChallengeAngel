using Axpo.PowerTradeForecast.Application.DTOs;
using FluentValidation;

namespace PowerTradeApp.Application.Validators
{
    public class HourlyVolumeValidator : AbstractValidator<HourlyVolume>
    {
        public HourlyVolumeValidator()
        {
            RuleFor(hv => hv.Datetime)
                .NotNull()
                .WithMessage("Datetime cannot be null.");

            RuleFor(hv => hv.Volume)
                .GreaterThan(0)
                .WithMessage("Volume must be greater than zero.");
        }
    }
}