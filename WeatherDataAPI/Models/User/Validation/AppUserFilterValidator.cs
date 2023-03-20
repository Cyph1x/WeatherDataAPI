using FluentValidation;
using MongoDB.Bson;

namespace WeatherDataAPI.Models.User.Validation
{
    public class AppUserFilterValidator : AbstractValidator<AppUserFilter>
    {
        public AppUserFilterValidator()
        {
            //check that the email is valid
            When(filter => !string.IsNullOrEmpty(filter.Email), () =>
            {
                RuleFor(filter => filter.Email).EmailAddress();
            });

            //check if the last access times are valid
            When(filter => filter.LastAccessedStart.HasValue, () =>
            {
                RuleFor(filter => filter.LastAccessedStart).GreaterThanOrEqualTo(DateTime.UnixEpoch);
            });
            When(filter => filter.LastAccessedEnd.HasValue, () =>
            {
                RuleFor(filter => filter.LastAccessedEnd).LessThanOrEqualTo(DateTime.MaxValue);
                RuleFor(filter => filter.LastAccessedEnd).GreaterThanOrEqualTo(filter => filter.LastAccessedStart);
                RuleFor(filter => filter.LastAccessedStart).LessThanOrEqualTo(filter => filter.LastAccessedEnd);
            });

            //check that the limit is within reasonable territory
            When(filter => filter.Limit.HasValue, () =>
            {
                RuleFor(filter => filter.Limit).InclusiveBetween(1, 10000);
            });

            //check that the offset is within reasonable territory
            When(filter => filter.Skip.HasValue, () =>
            {
                RuleFor(filter => filter.Skip).GreaterThanOrEqualTo(0);
            });

            //check if the Api Key is valid
            When(filter => !string.IsNullOrEmpty(filter.ApiKey), () =>
            {

                //check that Api Keys isnt set
                RuleFor(filter => filter.ApiKeys).Empty().WithMessage("'{PropertyName}' must be empty when Api Keys is set.");
                //check that the Api Key is valid
                RuleFor(filter => filter.ApiKey).Must(x => Guid.TryParse(x, out _)).WithMessage("'{PropertyName}' must be a valid Api Key. You entered {PropertyValue}");
            });
            //check that the Api Keys are valid
            When(filter => filter.ApiKeys != null && filter.ApiKeys.Count > 0, () =>
            {
                RuleFor(filter => filter.ApiKeys).Must(x => x.All(y => Guid.TryParse(y, out _))).WithMessage("'{PropertyName}' must contain only valid Api Keys.");
            });



            //Check that the id is valid
            When(filter => !string.IsNullOrEmpty(filter.Id), () =>
            {

                //check that ids isnt set
                RuleFor(filter => filter.Ids).Empty().WithMessage("'{PropertyName}' must be empty when id is set.");
                //check that the id is valid
                RuleFor(filter => filter.Id).Must(x => ObjectId.TryParse(x, out _)).WithMessage("'{PropertyName}' must be a valid id. You entered {PropertyValue}");
            });
            //check that the ids are valid
            When(filter => filter.Ids != null && filter.Ids.Count > 0, () =>
            {
                RuleFor(filter => filter.Ids).Must(x => x.All(y => ObjectId.TryParse(y, out _))).WithMessage("'{PropertyName}' must contain only valid ids.");
            });


        }
    }
}
