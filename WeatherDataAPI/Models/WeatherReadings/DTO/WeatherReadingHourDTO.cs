using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.WeatherReadings.DTO
{
    public class WeatherReadingHourDTO
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonPropertyName(WeatherReadingNames.Id)]
        [BsonElement(WeatherReadingNames.Id)]
        [JsonPropertyOrder(WeatherReadingOrders.Id)]
        public string Id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                _id = new ObjectId(value);
            }
        }
        [JsonPropertyName(WeatherReadingNames.Temperature)]
        [BsonElement(WeatherReadingNames.Temperature)]
        [JsonPropertyOrder(WeatherReadingOrders.Temperature)]
        public double? Temperature { get; set; }
        [JsonPropertyName(WeatherReadingNames.AtmosphericPressure)]
        [BsonElement(WeatherReadingNames.AtmosphericPressure)]
        [JsonPropertyOrder(WeatherReadingOrders.AtmosphericPressure)]
        public double? AtmosphericPressure { get; set; }

        [JsonPropertyName(WeatherReadingNames.SolarRadiation)]
        [BsonElement(WeatherReadingNames.SolarRadiation)]
        [JsonPropertyOrder(WeatherReadingOrders.SolarRadiation)]
        public double? SolarRadiation { get; set; }
        [JsonPropertyName(WeatherReadingNames.Precipitation)]
        [BsonElement(WeatherReadingNames.Precipitation)]
        [JsonPropertyOrder(WeatherReadingOrders.Precipitation)]
        public double? Precipitation { get; set; }
    }
}
