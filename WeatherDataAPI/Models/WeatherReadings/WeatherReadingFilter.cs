using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WeatherDataAPI.Models.WeatherReadings
{
    public class WeatherReadingFilter
    {

        public string? Id { get; set; }
        public List<string>? Ids { get; set; }
        public double? AtmosphericPressureMax { get; set; }
        public double? AtmosphericPressureMin { get; set; }
        public string? DeviceName { get; set; }
        [Range(0, 100)]
        public double? HumidityMax { get; set; }
        [Range(0, 100)]
        public double? HumidityMin { get; set; }
        [Range(-180, 180)]
        public double? LatitudeMax { get; set; }
        [Range(-180, 180)]
        public double? LatitudeMin { get; set; }
        [Range(-180, 180)]
        public double? LongitudeMax { get; set; }
        [Range(-180, 180)]
        public double? LongitudeMin { get; set; }
        public double? MaxWindSpeedMax { get; set; }
        public double? MaxWindSpeedMin { get; set; }
        public double? PrecipitationMax { get; set; }
        public double? PrecipitationMin { get; set; }
        public double? SolarRadiationMax { get; set; }
        public double? SolarRadiationMin { get; set; }
        public double? TemperatureMax { get; set; }
        public double? TemperatureMin { get; set; }
        /// <summary>
        /// Oldest time
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Newest Time
        /// </summary>
        public DateTime? EndTime { get; set; }
        public double? VaporPressureMax { get; set; }
        public double? VaporPressureMin { get; set; }
        public double? WindDirectionMax { get; set; }
        public double? WindDirectionMin { get; set; }
        /// <summary>
        /// Max amount to return
        /// </summary>
        public int? Limit { get; set; } = 100;
        /// <summary>
        /// amount of results to skip
        /// </summary>
        public int? Skip { get; set; }
        /// <summary>
        /// The key the results should be sorted by
        /// </summary>

        public string? SortBy { get; set; }
        /// <summary>
        /// Sort order asc/desc
        /// </summary>
        public string? SortOrder { get; set; }
        /// <summary>
        /// Fields to include
        /// </summary>
        public WeatherReadingProjection? Projection { get; set; }

        internal FilterDefinition<WeatherReading> BuildFilter()
        {
            FilterDefinitionBuilder<WeatherReading> builder = Builders<WeatherReading>.Filter;
            FilterDefinition<WeatherReading> filter = builder.Empty;
            if (!string.IsNullOrEmpty(Id))
            {
                filter &= builder.Eq("_id", new ObjectId(Id));
            }
            if (Ids != null)
            {
                if (Ids.Count > 0)
                {
                    filter &= builder.In("_id", Ids.Select(id => new ObjectId(id)));
                }
            }
            if (AtmosphericPressureMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.AtmosphericPressure, AtmosphericPressureMax.Value);
            }
            if (AtmosphericPressureMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.AtmosphericPressure, AtmosphericPressureMin.Value);
            }
            if (!string.IsNullOrEmpty(DeviceName))
            {
                filter &= builder.Regex(c => c.DeviceName, BsonRegularExpression.Create(Regex.Escape(DeviceName)));
            }
            if (HumidityMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Humidity, HumidityMax.Value);
            }
            if (HumidityMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Humidity, HumidityMin.Value);
            }
            if (LatitudeMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Latitude, LatitudeMax.Value);
            }
            if (LatitudeMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Latitude, LatitudeMin.Value);
            }
            if (LongitudeMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Longitude, LongitudeMax.Value);
            }
            if (LongitudeMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Longitude, LongitudeMin.Value);
            }
            if (MaxWindSpeedMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.MaxWindSpeed, MaxWindSpeedMax.Value);
            }
            if (MaxWindSpeedMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.MaxWindSpeed, MaxWindSpeedMin.Value);
            }
            if (PrecipitationMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Precipitation, PrecipitationMax.Value);
            }
            if (PrecipitationMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Precipitation, PrecipitationMin.Value);
            }
            if (SolarRadiationMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.SolarRadiation, SolarRadiationMax.Value);
            }
            if (SolarRadiationMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.SolarRadiation, SolarRadiationMin.Value);
            }
            if (TemperatureMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Temperature, TemperatureMax.Value);
            }
            if (TemperatureMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Temperature, TemperatureMin.Value);
            }
            if (StartTime.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.Time, StartTime.Value);
            }
            if (EndTime.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.Time, EndTime.Value);
            }
            if (VaporPressureMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.VaporPressure, VaporPressureMax.Value);
            }
            if (VaporPressureMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.VaporPressure, VaporPressureMin.Value);
            }
            if (WindDirectionMax.HasValue)
            {
                filter &= builder.Lte(WeatherReadingNames.WindDirection, WindDirectionMax.Value);
            }
            if (WindDirectionMin.HasValue)
            {
                filter &= builder.Gte(WeatherReadingNames.WindDirection, WindDirectionMin.Value);
            }

            return filter;
        }

        internal FindOptions<WeatherReading> BuildFindOptions()
        {
            if (SortOrder == null)
            {
                SortOrder = "asc";
            }
            FindOptions<WeatherReading> options = new FindOptions<WeatherReading>
            {
                Limit = Limit,
                Skip = Skip,
                Sort = SortBy != null ? WeatherReadingNames.names.ContainsKey(SortBy) ? new BsonDocument(WeatherReadingNames.names[SortBy], SortOrder.ToLower() == "desc" ? -1 : 1) : new BsonDocument(SortBy, SortOrder.ToLower() == "desc" ? -1 : 1) : null
            };
            if (Projection != null)
            {
                // convert the projection to a bson document
                var projection = new BsonDocument();
                foreach (var field in Projection.GetType().GetProperties())
                {
                    if (field.GetValue(Projection) != null)
                    {
                        if ((bool)field.GetValue(Projection))
                        {
                            projection.Add(WeatherReadingNames.names[field.Name], 1);

                        }
                    }
                }
                options.Projection = projection;
            }


            return options;
        }
    }
}
