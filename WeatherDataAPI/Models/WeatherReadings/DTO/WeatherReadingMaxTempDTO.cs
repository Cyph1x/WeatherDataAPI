﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.WeatherReadings.DTO
{
    public class WeatherReadingMaxTempDTO
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
        [JsonPropertyName(WeatherReadingNames.DeviceName)]
        [BsonElement(WeatherReadingNames.DeviceName)]
        [JsonPropertyOrder(WeatherReadingOrders.DeviceName)]
        public string DeviceName { get; set; }
        [JsonPropertyName(WeatherReadingNames.Temperature)]
        [BsonElement(WeatherReadingNames.Temperature)]
        [JsonPropertyOrder(WeatherReadingOrders.Temperature)]
        public double Temperature { get; set; }
        [JsonPropertyName(WeatherReadingNames.Time)]
        [BsonElement(WeatherReadingNames.Time)]
        [JsonPropertyOrder(WeatherReadingOrders.Time)]
        public DateTime Time { get; set; }

    }
}
