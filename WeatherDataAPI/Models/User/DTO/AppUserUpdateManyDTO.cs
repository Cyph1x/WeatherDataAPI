﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace WeatherDataAPI.Models.User.DTO
{
    public class AppUserUpdateManyDTO
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
        [JsonPropertyOrder(1)]
        public AppUserBase Update { get; set; }
    }
}
