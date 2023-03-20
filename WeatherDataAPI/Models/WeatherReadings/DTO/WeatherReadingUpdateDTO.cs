using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.WeatherReadings.DTO
{
    public class WeatherReadingUpdateDTO : WeatherReadingBase
    {

        public UpdateDefinition<WeatherReading> ToUpdateDefinition()
        {
            var type = GetType();
            var properties = type.GetProperties();
            UpdateDefinition<WeatherReading> update = null;
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(this);
                if (value != null)
                {
                    if (update == null)
                    {
                        update = Builders<WeatherReading>.Update.Set(name, value);
                    }
                    else
                    {
                        update = update.Set(name, value);
                    }
                }
            }
            return update;
        }
    }
    public class WeatherReadingUpdateManyDTO
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
        public WeatherReadingUpdateDTO Update { get; set; }
    }
}
