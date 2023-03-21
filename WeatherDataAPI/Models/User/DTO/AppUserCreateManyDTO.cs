using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.User.DTO
{
    public class AppUserCreateManyDTO : AppUserBase
    {

        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonPropertyName(AppUserNames.Id)]
        [BsonElement(AppUserNames.Id)]
        [JsonPropertyOrder(AppUserOrders.Id)]
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
}
