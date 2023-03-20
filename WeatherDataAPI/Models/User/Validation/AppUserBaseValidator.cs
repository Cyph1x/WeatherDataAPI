using FluentValidation;
using WeatherDataAPI.Models.User.DTO;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models.WeatherReadings.Validation;

namespace WeatherDataAPI.Models.User.Validation
{
    public class AppUserBaseValidator : AbstractValidator<AppUserBase>
    {
        public AppUserBaseValidator()
        {
            RuleSet("Update", () =>
            {
                When(user => user.Name != null, () =>
                {
                    RuleFor(user => user.Name).NotEmpty().MaximumLength(128);

                });
                When(user => user.Email != null, () =>
                {
                    RuleFor(user => user.Email).NotEmpty().EmailAddress().MaximumLength(128);
                });

            });
            RuleFor(user=>user.Name).NotEmpty().MaximumLength(128);
            RuleFor(user=>user.Email).NotEmpty().EmailAddress().MaximumLength(128);
            RuleFor(user=>user.Active).NotNull();
            RuleFor(user=>user.Permissions).NotNull();

        }
    }
    public class AppUserBaseManyValidator : AbstractValidator<List<AppUserBase>>
    {
        public AppUserBaseManyValidator()
        {
            RuleForEach(user => user).SetValidator(new AppUserBaseValidator());
        }
    }
}
