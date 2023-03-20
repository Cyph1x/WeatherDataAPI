using FluentValidation;
using WeatherDataAPI.Models.User.DTO;

namespace WeatherDataAPI.Models.User.Validation
{
    public class AppUserUpdateManyDTOValidator : AbstractValidator<AppUserUpdateManyDTO>
    {
        public AppUserUpdateManyDTOValidator()
        {
            RuleFor(AppUserUpdateManyDTO => AppUserUpdateManyDTO.Update).SetValidator(new AppUserBaseValidator());
        }
    }
    public class AppUserUpdateManyDTOManyValidator : AbstractValidator<List<AppUserUpdateManyDTO>>
    {
        public AppUserUpdateManyDTOManyValidator()
        {
            RuleForEach(AppUserUpdateManyDTO => AppUserUpdateManyDTO).SetValidator(new AppUserUpdateManyDTOValidator());
        }
    }
}
