using FluentValidation;
using MongoDB.Bson;

namespace WeatherDataAPI.Models.WeatherReadings.Validation
{
    public class WeatherReadingFilterValidator : AbstractValidator<WeatherReadingFilter>
    {
        public WeatherReadingFilterValidator()
        {
            //check if atmospheric min and max are valid
            When(filter => filter.AtmosphericPressureMax.HasValue, () =>
            {
                RuleFor(filter => filter.AtmosphericPressureMax).GreaterThanOrEqualTo(0);

            });
            When(filter => filter.AtmosphericPressureMin.HasValue, () =>
            {
                RuleFor(filter => filter.AtmosphericPressureMin).GreaterThanOrEqualTo(0);
                When(filter => filter.AtmosphericPressureMax.HasValue, () =>
                {
                    RuleFor(filter => filter.AtmosphericPressureMax).GreaterThanOrEqualTo(filter => filter.AtmosphericPressureMin);
                    RuleFor(filter => filter.AtmosphericPressureMin).LessThanOrEqualTo(filter => filter.AtmosphericPressureMax);
                });
            });

            //check if the device name is too long
            RuleFor(filter => filter.DeviceName).MaximumLength(128);

            //check if humidity min and max are valid
            When(filter => filter.HumidityMax.HasValue, () =>
            {
                RuleFor(filter => filter.HumidityMax).InclusiveBetween(0, 100);
            });
            When(filter => filter.HumidityMin.HasValue, () =>
            {
                RuleFor(filter => filter.HumidityMin).InclusiveBetween(0, 100);
                When(filter => filter.HumidityMax.HasValue, () =>
                {
                    RuleFor(filter => filter.HumidityMax).GreaterThanOrEqualTo(filter => filter.HumidityMin);
                    RuleFor(filter => filter.HumidityMin).LessThanOrEqualTo(filter => filter.HumidityMax);
                });
            });

            //check if latitude min and max are valid
            When(filter => filter.LatitudeMax.HasValue, () =>
            {
                RuleFor(filter => filter.LatitudeMax).InclusiveBetween(-180, 180);
            });
            When(filter => filter.LatitudeMin.HasValue, () =>
            {
                RuleFor(filter => filter.LatitudeMin).InclusiveBetween(-180, 180);
                When(filter => filter.LatitudeMax.HasValue, () =>
                {
                    RuleFor(filter => filter.LatitudeMax).GreaterThanOrEqualTo(filter => filter.LatitudeMin);
                    RuleFor(filter => filter.LatitudeMin).LessThanOrEqualTo(filter => filter.LatitudeMax);
                });
            });

            //check if longitude min and max are valid
            When(filter => filter.LongitudeMax.HasValue, () =>
            {
                RuleFor(filter => filter.LongitudeMax).InclusiveBetween(-180, 180);
            });
            When(filter => filter.LongitudeMin.HasValue, () =>
            {
                RuleFor(filter => filter.LongitudeMin).InclusiveBetween(-180, 180);
                When(filter => filter.LongitudeMax.HasValue, () =>
                {
                    RuleFor(filter => filter.LongitudeMax).GreaterThanOrEqualTo(filter => filter.LongitudeMin);
                    RuleFor(filter => filter.LongitudeMin).LessThanOrEqualTo(filter => filter.LongitudeMax);
                });
            });

            //check if max wind speed min and max are valid
            When(filter => filter.MaxWindSpeedMax.HasValue, () =>
            {
                RuleFor(filter => filter.MaxWindSpeedMax).GreaterThanOrEqualTo(0);
            });
            When(filter => filter.MaxWindSpeedMin.HasValue, () =>
            {
                RuleFor(filter => filter.MaxWindSpeedMin).GreaterThanOrEqualTo(0);
                When(filter => filter.MaxWindSpeedMax.HasValue, () =>
                {
                    RuleFor(filter => filter.MaxWindSpeedMax).GreaterThanOrEqualTo(filter => filter.MaxWindSpeedMin);
                    RuleFor(filter => filter.MaxWindSpeedMin).LessThanOrEqualTo(filter => filter.MaxWindSpeedMax);
                });
            });

            //check if precipitation min and max are valid
            When(filter => filter.PrecipitationMax.HasValue, () =>
            {
                RuleFor(filter => filter.PrecipitationMax).GreaterThanOrEqualTo(0);
            });
            When(filter => filter.PrecipitationMin.HasValue, () =>
            {
                RuleFor(filter => filter.PrecipitationMin).GreaterThanOrEqualTo(0);
                When(filter => filter.PrecipitationMax.HasValue, () =>
                {
                    RuleFor(filter => filter.PrecipitationMax).GreaterThanOrEqualTo(filter => filter.PrecipitationMin);
                    RuleFor(filter => filter.PrecipitationMin).LessThanOrEqualTo(filter => filter.PrecipitationMax);
                });
            });

            //check if solar radiation min and max are valid
            When(filter => filter.SolarRadiationMax.HasValue, () =>
            {
                RuleFor(filter => filter.SolarRadiationMax).GreaterThanOrEqualTo(0);
            });
            When(filter => filter.SolarRadiationMin.HasValue, () =>
            {
                RuleFor(filter => filter.SolarRadiationMin).GreaterThanOrEqualTo(0);
                When(filter => filter.SolarRadiationMax.HasValue, () =>
                {
                    RuleFor(filter => filter.SolarRadiationMax).GreaterThanOrEqualTo(filter => filter.SolarRadiationMin);
                    RuleFor(filter => filter.SolarRadiationMin).LessThanOrEqualTo(filter => filter.SolarRadiationMax);
                });
            });

            //check if temperature min and max are valid
            When(filter => filter.TemperatureMax.HasValue, () =>
            {
                RuleFor(filter => filter.TemperatureMax).InclusiveBetween(-50, 60);
            });
            When(filter => filter.TemperatureMin.HasValue, () =>
            {
                RuleFor(filter => filter.TemperatureMin).InclusiveBetween(-50, 60);
                When(filter => filter.TemperatureMax.HasValue, () =>
                {
                    RuleFor(filter => filter.TemperatureMax).GreaterThanOrEqualTo(filter => filter.TemperatureMin);
                    RuleFor(filter => filter.TemperatureMin).LessThanOrEqualTo(filter => filter.TemperatureMax);
                });
            });

            //check if the start and end times are valid
            When(filter => filter.StartTime.HasValue, () =>
            {
                RuleFor(filter => filter.StartTime).GreaterThanOrEqualTo(DateTime.UnixEpoch);
            });
            When(filter => filter.EndTime.HasValue, () =>
            {
                RuleFor(filter => filter.EndTime).LessThanOrEqualTo(DateTime.MaxValue);
                RuleFor(filter => filter.EndTime).GreaterThanOrEqualTo(filter => filter.StartTime);
                RuleFor(filter => filter.StartTime).LessThanOrEqualTo(filter => filter.EndTime);
            });

            //check if vapour pressure min and max are valid
            When(filter => filter.VaporPressureMax.HasValue, () =>
            {
                RuleFor(filter => filter.VaporPressureMax).GreaterThanOrEqualTo(0);
            });
            When(filter => filter.VaporPressureMin.HasValue, () =>
            {
                RuleFor(filter => filter.VaporPressureMin).GreaterThanOrEqualTo(0);
                When(filter => filter.VaporPressureMax.HasValue, () =>
                {
                    RuleFor(filter => filter.VaporPressureMax).GreaterThanOrEqualTo(filter => filter.VaporPressureMin);
                    RuleFor(filter => filter.VaporPressureMin).LessThanOrEqualTo(filter => filter.VaporPressureMax);
                });
            });

            //check if wind direction min and max are valid
            When(filter => filter.WindDirectionMax.HasValue, () =>
            {
                RuleFor(filter => filter.WindDirectionMax).InclusiveBetween(0, 360);
            });
            When(filter => filter.WindDirectionMin.HasValue, () =>
            {
                RuleFor(filter => filter.WindDirectionMin).InclusiveBetween(0, 360);
                When(filter => filter.WindDirectionMax.HasValue, () =>
                {
                    RuleFor(filter => filter.WindDirectionMax).GreaterThanOrEqualTo(filter => filter.WindDirectionMin);
                    RuleFor(filter => filter.WindDirectionMin).LessThanOrEqualTo(filter => filter.WindDirectionMax);
                });
            });

            //check that the limit is within reasonable territory
            When(filter => filter.Limit.HasValue, () =>
            {
                RuleFor(filter => filter.Limit).InclusiveBetween(1, 10000);
            });

            //check that the offset is greater than 0
            When(filter => filter.Skip.HasValue, () =>
            {
                RuleFor(filter => filter.Skip).GreaterThanOrEqualTo(0);
            });

            //check if the Sort key is valid

            When(filter => !string.IsNullOrEmpty(filter.SortBy), () =>
            {
                RuleFor(x => x.SortBy).Must(x =>
                {
                    return WeatherReadingNames.validNames.Contains(x);
                }).WithState(x => WeatherReadingNames.names.Values).WithMessage("'{PropertyName}' must be equal a valid sort key. You entered {PropertyValue}");
            });

            //check that sort order is valid
            When(filter => !string.IsNullOrEmpty(filter.SortOrder), () =>
            {
                RuleFor(x => x.SortOrder).Must(x =>
                {
                    return x.ToLower() == "asc" || x.ToLower() == "desc";
                }).WithMessage("'{PropertyName}' must be equal to 'asc' or 'desc'. You entered {PropertyValue}");
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
    public class WeatherReadingFilterManyValidator : AbstractValidator<List<WeatherReadingFilter>>
    {
        public WeatherReadingFilterManyValidator()
        {
            RuleForEach(WeatherReadingFilter => WeatherReadingFilter).SetValidator(new WeatherReadingFilterValidator());
        }
    }

}
