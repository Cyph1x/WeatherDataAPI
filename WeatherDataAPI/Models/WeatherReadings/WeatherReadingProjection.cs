using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.WeatherReadings
{
    public class WeatherReadingProjection
    {

        [JsonPropertyName(WeatherReadingNames.AtmosphericPressure)]
        [BsonElement(WeatherReadingNames.AtmosphericPressure)]
        [JsonPropertyOrder(WeatherReadingOrders.AtmosphericPressure)]
        public bool? AtmosphericPressure { get; set; }
        [JsonPropertyName(WeatherReadingNames.DeviceName)]
        [BsonElement(WeatherReadingNames.DeviceName)]
        [JsonPropertyOrder(WeatherReadingOrders.DeviceName)]
        public bool? DeviceName { get; set; }
        [JsonPropertyName(WeatherReadingNames.Humidity)]
        [BsonElement(WeatherReadingNames.Humidity)]
        [JsonPropertyOrder(WeatherReadingOrders.Humidity)]
        public bool? Humidity { get; set; }
        [JsonPropertyName(WeatherReadingNames.Latitude)]
        [BsonElement(WeatherReadingNames.Latitude)]
        [JsonPropertyOrder(WeatherReadingOrders.Latitude)]
        public bool? Latitude { get; set; }
        [JsonPropertyName(WeatherReadingNames.Longitude)]
        [BsonElement(WeatherReadingNames.Longitude)]
        [JsonPropertyOrder(WeatherReadingOrders.Longitude)]
        public bool? Longitude { get; set; }
        [JsonPropertyName(WeatherReadingNames.MaxWindSpeed)]
        [BsonElement(WeatherReadingNames.MaxWindSpeed)]
        [JsonPropertyOrder(WeatherReadingOrders.MaxWindSpeed)]
        public bool? MaxWindSpeed { get; set; }
        [JsonPropertyName(WeatherReadingNames.Precipitation)]
        [BsonElement(WeatherReadingNames.Precipitation)]
        [JsonPropertyOrder(WeatherReadingOrders.Precipitation)]
        public bool? Precipitation { get; set; }
        [JsonPropertyName(WeatherReadingNames.SolarRadiation)]
        [BsonElement(WeatherReadingNames.SolarRadiation)]
        [JsonPropertyOrder(WeatherReadingOrders.SolarRadiation)]
        public bool? SolarRadiation { get; set; }
        [JsonPropertyName(WeatherReadingNames.Temperature)]
        [BsonElement(WeatherReadingNames.Temperature)]
        [JsonPropertyOrder(WeatherReadingOrders.Temperature)]

        public bool? Temperature { get; set; }
        [JsonPropertyName(WeatherReadingNames.Time)]
        [BsonElement(WeatherReadingNames.Time)]
        [JsonPropertyOrder(WeatherReadingOrders.Time)]
        public bool? Time { get; set; }
        [JsonPropertyName(WeatherReadingNames.VaporPressure)]
        [BsonElement(WeatherReadingNames.VaporPressure)]
        [JsonPropertyOrder(WeatherReadingOrders.VaporPressure)]
        public bool? VaporPressure { get; set; }
        [JsonPropertyName(WeatherReadingNames.WindDirection)]
        [BsonElement(WeatherReadingNames.WindDirection)]
        [JsonPropertyOrder(WeatherReadingOrders.WindDirection)]
        public bool? WindDirection { get; set; }


    }
}
