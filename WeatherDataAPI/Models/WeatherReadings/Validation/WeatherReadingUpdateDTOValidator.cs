using FluentValidation;
using MongoDB.Bson;
using WeatherDataAPI.Models.WeatherReadings.DTO;

namespace WeatherDataAPI.Models.WeatherReadings.Validation
{
    public class WeatherReadingUpdateDTOValidator : AbstractValidator<WeatherReadingUpdateDTO>
    {
        public WeatherReadingUpdateDTOValidator()
        {
            Include(new WeatherReadingBaseValidator());
        }
    }
    public class WeatherReadingUpdateDTOManyValidator : AbstractValidator<List<WeatherReadingUpdateDTO>>
    {
        public WeatherReadingUpdateDTOManyValidator()
        {
            RuleForEach(WeatherReadingUpdateDTO => WeatherReadingUpdateDTO).SetValidator(new WeatherReadingUpdateDTOValidator());
        }
    }
    public class WeatherReadingUpdateManyDTOValidator : AbstractValidator<WeatherReadingUpdateManyDTO>
    {
        public WeatherReadingUpdateManyDTOValidator()
        {
            //check the id is valid
            RuleFor(WeatherReadingUpdateManyDTO => WeatherReadingUpdateManyDTO.Id).NotEmpty();
            RuleFor(WeatherReadingUpdateManyDTO => WeatherReadingUpdateManyDTO.Id).Must(x => ObjectId.TryParse(x, out _)).WithMessage("'{PropertyName}' must be a valid id. You entered {PropertyValue}");
            //check the update is valid
            RuleFor(WeatherReadingUpdateManyDTO => WeatherReadingUpdateManyDTO.Update).SetValidator(new WeatherReadingUpdateDTOValidator());

        }
    }
    public class WeatherReadingUpdateManyDTOManyValidator : AbstractValidator<List<WeatherReadingUpdateManyDTO>>
    {
        public WeatherReadingUpdateManyDTOManyValidator()
        {
            RuleForEach(WeatherReadingUpdateManyDTO => WeatherReadingUpdateManyDTO).SetValidator(new WeatherReadingUpdateManyDTOValidator());
        }
    }
}
