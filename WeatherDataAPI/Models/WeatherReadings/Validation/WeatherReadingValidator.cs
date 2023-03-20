using FluentValidation;
using MongoDB.Bson;


namespace WeatherDataAPI.Models.WeatherReadings.Validation
{
    public class WeatherReadingValidator : AbstractValidator<WeatherReading>
    {
        public WeatherReadingValidator()
        {
            Include(new WeatherReadingBaseValidator());
            RuleFor(WeatherReading => WeatherReading.Id).NotEmpty();
            RuleFor(WeatherReading => WeatherReading.Id).Must(x => ObjectId.TryParse(x, out _)).WithMessage("'{PropertyName}' must be a valid id. You entered {PropertyValue}");
        }
    }
    public class WeatherReadingManyValidator : AbstractValidator<List<WeatherReading>>
    {

        public WeatherReadingManyValidator()
        {
            RuleForEach(WeatherReading => WeatherReading).SetValidator(new WeatherReadingValidator());
        }
    }
}
