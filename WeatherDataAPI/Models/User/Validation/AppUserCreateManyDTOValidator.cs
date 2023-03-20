using FluentValidation;
using WeatherDataAPI.Models.User.DTO;

namespace WeatherDataAPI.Models.User.Validation
{
    public class AppUserCreateManyDTOValidator : AbstractValidator<AppUserCreateManyDTO>
    {
        public AppUserCreateManyDTOValidator()
        {
            Include(new AppUserBaseValidator());
        }
    }
    public class AppUserCreateManyDTOManyValidator : AbstractValidator<List<AppUserCreateManyDTO>>
    {
        public AppUserCreateManyDTOManyValidator()
        {
            RuleForEach(user => user).SetValidator(new AppUserCreateManyDTOValidator());
        }
    }
}
