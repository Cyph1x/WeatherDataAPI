using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.AppUser
{
    public class Permissions
    {
        [JsonPropertyName(PermissionsNames.Admin)]
        [BsonElement(PermissionsNames.Admin)]
        [JsonPropertyOrder(PermissionsOrders.Admin)]
        public bool? Admin { get; set; } = false;
        [JsonPropertyName(PermissionsNames.GetWeatherReading)]
        [BsonElement(PermissionsNames.GetWeatherReading)]
        [JsonPropertyOrder(PermissionsOrders.GetWeatherReading)]
        public bool? GetWeatherReading { get; set; }
        [JsonPropertyName(PermissionsNames.GetWeatherReadings)]
        [BsonElement(PermissionsNames.GetWeatherReadings)]
        [JsonPropertyOrder(PermissionsOrders.GetWeatherReadings)]
        public bool? GetWeatherReadings { get; set; }
        [JsonPropertyName(PermissionsNames.PostWeatherReading)]
        [BsonElement(PermissionsNames.PostWeatherReading)]
        [JsonPropertyOrder(PermissionsOrders.PostWeatherReading)]
        public bool? PostWeatherReading { get; set; }
        [JsonPropertyName(PermissionsNames.PostWeatherReadings)]
        [BsonElement(PermissionsNames.PostWeatherReadings)]
        [JsonPropertyOrder(PermissionsOrders.PostWeatherReadings)]
        public bool? PostWeatherReadings { get; set; }
        [JsonPropertyName(PermissionsNames.DeleteWeatherReading)]
        [BsonElement(PermissionsNames.DeleteWeatherReading)]
        [JsonPropertyOrder(PermissionsOrders.DeleteWeatherReading)]
        public bool? DeleteWeatherReading { get; set; }
        [JsonPropertyName(PermissionsNames.DeleteWeatherReadings)]
        [BsonElement(PermissionsNames.DeleteWeatherReadings)]
        [JsonPropertyOrder(PermissionsOrders.DeleteWeatherReadings)]
        public bool? DeleteWeatherReadings { get; set; }
        [JsonPropertyName(PermissionsNames.PutWeatherReading)]
        [BsonElement(PermissionsNames.PutWeatherReading)]
        [JsonPropertyOrder(PermissionsOrders.PutWeatherReading)]
        public bool? PutWeatherReading { get; set; }
        [JsonPropertyName(PermissionsNames.PutWeatherReadings)]
        [BsonElement(PermissionsNames.PutWeatherReadings)]
        [JsonPropertyOrder(PermissionsOrders.PutWeatherReadings)]
        public bool? PutWeatherReadings { get; set; }
        [JsonPropertyName(PermissionsNames.PatchWeatherReading)]
        [BsonElement(PermissionsNames.PatchWeatherReading)]
        [JsonPropertyOrder(PermissionsOrders.PatchWeatherReading)]
        public bool? PatchWeatherReading { get; set; }
        [JsonPropertyName(PermissionsNames.PatchWeatherReadings)]
        [BsonElement(PermissionsNames.PatchWeatherReadings)]
        [JsonPropertyOrder(PermissionsOrders.PatchWeatherReadings)]
        public bool? PatchWeatherReadings { get; set; }
        [JsonPropertyName(PermissionsNames.GetMaxPrecipitationForLastFiveMonths)]
        [BsonElement(PermissionsNames.GetMaxPrecipitationForLastFiveMonths)]
        [JsonPropertyOrder(PermissionsOrders.GetMaxPrecipitationForLastFiveMonths)]
        public bool? GetMaxPrecipitationForLastFiveMonths { get; set; }
        [JsonPropertyName(PermissionsNames.GetReadingsAtSpecificHourForStation)]
        [BsonElement(PermissionsNames.GetReadingsAtSpecificHourForStation)]
        [JsonPropertyOrder(PermissionsOrders.GetReadingsAtSpecificHourForStation)]
        public bool? GetReadingsAtSpecificHourForStation { get; set; }
        [JsonPropertyName(PermissionsNames.GetMaxTempBetweenTimes)]
        [BsonElement(PermissionsNames.GetMaxTempBetweenTimes)]
        [JsonPropertyOrder(PermissionsOrders.GetMaxTempBetweenTimes)]
        public bool? GetMaxTempBetweenTimes { get; set; }



        public bool checkPermission(string permission)
        {
            try
            {
                if ((bool)Admin)
                {
                    return true;
                }
            }
            catch { }
            var type = GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                if (name == permission)
                {
                    return (bool)property.GetValue(this);
                }
            }
            return false;

        }

    }
    public static class PermissionsNames
    {
        public const string Admin = "Admin";
        public const string GetWeatherReading = "GetWeatherReading";
        public const string GetWeatherReadings = "GetWeatherReadings";
        public const string PostWeatherReading = "PostWeatherReading";
        public const string PostWeatherReadings = "PostWeatherReadings";
        public const string DeleteWeatherReading = "DeleteWeatherReading";
        public const string DeleteWeatherReadings = "DeleteWeatherReadings";
        public const string PutWeatherReading = "PutWeatherReading";
        public const string PutWeatherReadings = "PutWeatherReadings";
        public const string PatchWeatherReading = "PatchWeatherReading";
        public const string PatchWeatherReadings = "PatchWeatherReadings";
        public const string GetMaxPrecipitationForLastFiveMonths = "GetMaxPrecipitationForLastFiveMonths";
        public const string GetReadingsAtSpecificHourForStation = "GetReadingsAtSpecificHourForStation";
        public const string GetMaxTempBetweenTimes = "GetMaxTempBetweenTimes";
        public static readonly IReadOnlyDictionary<string, string> names = typeof(Permissions).GetProperties().Select(p => new { Property = p, Attribute = p.GetCustomAttributes(typeof(BsonElementAttribute), true).Cast<BsonElementAttribute>().FirstOrDefault() }).Where(p => p.Attribute != null).ToDictionary(p => p.Property.Name, p => p.Attribute.ElementName);


        public static readonly string[] validNames = names.Keys.ToList().Concat(names.Values.ToList()).ToArray();
    }
    public static class PermissionsOrders
    {
        public const int Admin = 0;
        public const int GetWeatherReading = 1;
        public const int GetWeatherReadings = 2;
        public const int PostWeatherReading = 3;
        public const int PostWeatherReadings = 4;
        public const int DeleteWeatherReading = 5;
        public const int DeleteWeatherReadings = 6;
        public const int PutWeatherReading = 7;
        public const int PutWeatherReadings = 8;
        public const int PatchWeatherReading = 9;
        public const int PatchWeatherReadings = 10;
        public const int GetMaxPrecipitationForLastFiveMonths = 11;
        public const int GetReadingsAtSpecificHourForStation = 12;
        public const int GetMaxTempBetweenTimes = 13;
    }
}
