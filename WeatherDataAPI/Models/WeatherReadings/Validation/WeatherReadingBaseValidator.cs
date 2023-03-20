using FluentValidation;

namespace WeatherDataAPI.Models.WeatherReadings.Validation
{
    public class WeatherReadingBaseValidator : AbstractValidator<WeatherReadingBase>
    {
        public WeatherReadingBaseValidator()
        {
            RuleSet("Update", () =>
            {
                RuleFor(WeatherReading => WeatherReading.DeviceName).MaximumLength(128);
                RuleFor(WeatherReading => WeatherReading.Latitude).InclusiveBetween(-180, 180);
                RuleFor(WeatherReading => WeatherReading.Longitude).InclusiveBetween(-180, 180);
                RuleFor(WeatherReading => WeatherReading.Temperature).InclusiveBetween(-50, 60);
                RuleFor(WeatherReading => WeatherReading.Humidity).InclusiveBetween(0, 100);
                RuleFor(WeatherReading => WeatherReading.WindDirection).InclusiveBetween(0, 360);
                RuleFor(WeatherReading => WeatherReading.MaxWindSpeed).GreaterThanOrEqualTo(0);
                RuleFor(WeatherReading => WeatherReading.Precipitation).GreaterThanOrEqualTo(0);
                RuleFor(WeatherReading => WeatherReading.SolarRadiation).GreaterThanOrEqualTo(0);
                RuleFor(WeatherReading => WeatherReading.VaporPressure).GreaterThanOrEqualTo(0);
                RuleFor(WeatherReading => WeatherReading.AtmosphericPressure).GreaterThanOrEqualTo(0);
            });
            RuleFor(WeatherReading => WeatherReading.DeviceName).NotEmpty().MaximumLength(128);
            RuleFor(WeatherReading => WeatherReading.Latitude).InclusiveBetween(-180, 180);
            RuleFor(WeatherReading => WeatherReading.Longitude).InclusiveBetween(-180, 180);
            RuleFor(WeatherReading => WeatherReading.Temperature).InclusiveBetween(-50, 60);
            RuleFor(WeatherReading => WeatherReading.Humidity).InclusiveBetween(0, 100);
            RuleFor(WeatherReading => WeatherReading.WindDirection).InclusiveBetween(0, 360);
            RuleFor(WeatherReading => WeatherReading.MaxWindSpeed).GreaterThanOrEqualTo(0);
            RuleFor(WeatherReading => WeatherReading.Precipitation).GreaterThanOrEqualTo(0);
            RuleFor(WeatherReading => WeatherReading.SolarRadiation).GreaterThanOrEqualTo(0);
            RuleFor(WeatherReading => WeatherReading.VaporPressure).GreaterThanOrEqualTo(0);
            RuleFor(WeatherReading => WeatherReading.AtmosphericPressure).GreaterThanOrEqualTo(0);

        }
    }
    public class WeatherReadingBaseManyValidator : AbstractValidator<List<WeatherReadingBase>>
    {
        public WeatherReadingBaseManyValidator()
        {
            RuleForEach(WeatherReading => WeatherReading).SetValidator(new WeatherReadingBaseValidator());
        }
    }
}
