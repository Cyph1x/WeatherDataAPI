using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.WeatherReadings
{
    public class WeatherReadingBase
    {
        [JsonPropertyName(WeatherReadingNames.AtmosphericPressure)]
        [BsonElement(WeatherReadingNames.AtmosphericPressure)]
        [JsonPropertyOrder(WeatherReadingOrders.AtmosphericPressure)]
        public double? AtmosphericPressure { get; set; }
        [JsonPropertyName(WeatherReadingNames.DeviceName)]
        [BsonElement(WeatherReadingNames.DeviceName)]
        [JsonPropertyOrder(WeatherReadingOrders.DeviceName)]
        public string? DeviceName { get; set; }
        [JsonPropertyName(WeatherReadingNames.Humidity)]
        [BsonElement(WeatherReadingNames.Humidity)]
        [JsonPropertyOrder(WeatherReadingOrders.Humidity)]
        public double? Humidity { get; set; }
        [JsonPropertyName(WeatherReadingNames.Latitude)]
        [BsonElement(WeatherReadingNames.Latitude)]
        [JsonPropertyOrder(WeatherReadingOrders.Latitude)]
        public double? Latitude { get; set; }
        [JsonPropertyName(WeatherReadingNames.Longitude)]
        [BsonElement(WeatherReadingNames.Longitude)]
        [JsonPropertyOrder(WeatherReadingOrders.Longitude)]
        public double? Longitude { get; set; }
        [JsonPropertyName(WeatherReadingNames.MaxWindSpeed)]
        [BsonElement(WeatherReadingNames.MaxWindSpeed)]
        [JsonPropertyOrder(WeatherReadingOrders.MaxWindSpeed)]
        public double? MaxWindSpeed { get; set; }
        [JsonPropertyName(WeatherReadingNames.Precipitation)]
        [BsonElement(WeatherReadingNames.Precipitation)]
        [JsonPropertyOrder(WeatherReadingOrders.Precipitation)]
        public double? Precipitation { get; set; }
        [JsonPropertyName(WeatherReadingNames.SolarRadiation)]
        [BsonElement(WeatherReadingNames.SolarRadiation)]
        [JsonPropertyOrder(WeatherReadingOrders.SolarRadiation)]
        public double? SolarRadiation { get; set; }
        [JsonPropertyName(WeatherReadingNames.Temperature)]
        [BsonElement(WeatherReadingNames.Temperature)]
        [JsonPropertyOrder(WeatherReadingOrders.Temperature)]

        public double? Temperature { get; set; }
        [JsonPropertyName(WeatherReadingNames.Time)]
        [BsonElement(WeatherReadingNames.Time)]
        [JsonPropertyOrder(WeatherReadingOrders.Time)]
        public DateTime? Time { get; set; }
        [JsonPropertyName(WeatherReadingNames.VaporPressure)]
        [BsonElement(WeatherReadingNames.VaporPressure)]
        [JsonPropertyOrder(WeatherReadingOrders.VaporPressure)]
        public double? VaporPressure { get; set; }
        [JsonPropertyName(WeatherReadingNames.WindDirection)]
        [BsonElement(WeatherReadingNames.WindDirection)]
        [JsonPropertyOrder(WeatherReadingOrders.WindDirection)]
        public double? WindDirection { get; set; }

        internal WeatherReading ToReading()
        {
            //done for upgradability in future
            WeatherReading newWeatherReading = new WeatherReading();
            foreach (var property in GetType().GetProperties())
            {

                var value = property.GetValue(this);
                if (value != null)
                {
                    newWeatherReading.GetType().GetProperty(property.Name).SetValue(newWeatherReading, value);
                }
            }
            return newWeatherReading;


        }
    }
    public class WeatherReading : WeatherReadingBase
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonPropertyName(WeatherReadingNames.Id)]
        [BsonIgnore]
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







    }
    public static class WeatherReadingNames
    {
        public const string Id = "Id";
        public const string AtmosphericPressure = "Atmospheric Pressure (kPa)";
        public const string DeviceName = "Device Name";
        public const string Humidity = "Humidity (%)";
        public const string Latitude = "Latitude";
        public const string Longitude = "Longitude";
        public const string MaxWindSpeed = "Max Wind Speed (m/s)";
        public const string Precipitation = "Precipitation mm/h";
        public const string SolarRadiation = "Solar Radiation (W/m2)";
        public const string Temperature = "Temperature (°C)";
        public const string Time = "Time";
        public const string VaporPressure = "Vapor Pressure (kPa)";
        public const string WindDirection = "Wind Direction (°)";
        public static readonly IReadOnlyDictionary<string, string> names = typeof(WeatherReading).GetProperties().Select(p => new { Property = p, Attribute = p.GetCustomAttributes(typeof(BsonElementAttribute), true).Cast<BsonElementAttribute>().FirstOrDefault() }).Where(p => p.Attribute != null).ToDictionary(p => p.Property.Name, p => p.Attribute.ElementName);


        public static readonly string[] validNames = names.Keys.ToList().Concat(names.Values.ToList()).ToArray();
    }
    public static class WeatherReadingOrders
    {
        public const int Id = 0;
        public const int AtmosphericPressure = 1;
        public const int DeviceName = 2;
        public const int Humidity = 3;
        public const int Latitude = 4;
        public const int Longitude = 5;
        public const int MaxWindSpeed = 6;
        public const int Precipitation = 7;
        public const int SolarRadiation = 8;
        public const int Temperature = 9;
        public const int Time = 10;
        public const int VaporPressure = 11;
        public const int WindDirection = 12;
    }


}
