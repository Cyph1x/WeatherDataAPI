using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WeatherDataAPI.Models.WeatherReadings;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using WeatherDataAPI.Models.AppUser;
using System.ComponentModel.DataAnnotations;

namespace WeatherDataAPI.Models.User
{
    public class AppUserBase
    {
        [JsonPropertyName(AppUserNames.Name)]
        [BsonElement(AppUserNames.Name)]
        [JsonPropertyOrder(AppUserOrders.Name)]
        public string? Name { get; set; }
        /// <summary>
        ///  Must be unique
        /// </summary>
        [JsonPropertyName(AppUserNames.Email)]
        [BsonElement(AppUserNames.Email)]
        [JsonPropertyOrder(AppUserOrders.Email)]
        [EmailAddress]
        public string? Email { get; set; }
        /// <summary>
        ///  enable/disable
        /// </summary>
        [JsonPropertyName(AppUserNames.Active)]
        [BsonElement(AppUserNames.Active)]
        [JsonPropertyOrder(AppUserOrders.Active)]
        public bool? Active { get; set; }
        /// <summary>
        ///  Endpoints the user can access
        /// </summary>
        [JsonPropertyName(AppUserNames.Permissions)]
        [BsonElement(AppUserNames.Permissions)]
        [JsonPropertyOrder(AppUserOrders.Permissions)]
        
        public Permissions? Permissions { get; set; }


        internal AppUser ToAppUser()
        {
            //done for upgradability in future
            AppUser newAppUser = new AppUser();
            foreach (var property in GetType().GetProperties())
            {

                var value = property.GetValue(this);
                if (value != null)
                {
                    newAppUser.GetType().GetProperty(property.Name).SetValue(newAppUser, value);
                }
            }
            return newAppUser;


        }
        internal UpdateDefinition<AppUser> ToUpdateDefinition()
        {
            var type = GetType();
            var properties = type.GetProperties();
            UpdateDefinition<AppUser> update = null;
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(this);
                if (value != null)
                {

                    {
                        if (name == "Permissions")
                        {


                            foreach (var permissionProperty in typeof(Permissions).GetProperties())
                            {
                                var permissionName = permissionProperty.Name;
                                if (permissionProperty.GetValue(Permissions) != null)
                                {
                                    if (update == null)
                                    {
                                        update = Builders<AppUser>.Update.Set("Permissions." + permissionName, (bool)permissionProperty.GetValue(Permissions));
                                    }
                                    else
                                    {
                                        update = update.Set("Permissions." + permissionName, (bool)permissionProperty.GetValue(Permissions));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (update == null)
                            {
                                update = Builders<AppUser>.Update.Set(name, value);
                            }
                            else
                            {
                                update = update.Set(name, value);
                            }
                        }

                    }
                }
            }
            return update;
        }
    }
    public class AppUser: AppUserBase
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonPropertyName(AppUserNames.Id)]
        [BsonIgnore]
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
        [JsonPropertyName(AppUserNames.LastAccess)]
        [BsonElement(AppUserNames.LastAccess)]
        [JsonPropertyOrder(AppUserOrders.LastAccess)]
        public DateTime? LastAccess { get; set; } = DateTime.Now;
        [JsonPropertyName(AppUserNames.ApiKey)]
        [BsonElement(AppUserNames.ApiKey)]
        [JsonPropertyOrder(AppUserOrders.ApiKey)]
        public string ApiKey { get; set; } = Guid.NewGuid().ToString();


    }
    public static class AppUserNames
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Email = "Email";
        public const string Active = "Active";
        public const string Permissions = "Permissions";
        public const string LastAccess = "Last Access";
        public const string ApiKey = "Api Key";
        public static readonly IReadOnlyDictionary<string, string> names = typeof(AppUser).GetProperties().Select(p => new { Property = p, Attribute = p.GetCustomAttributes(typeof(BsonElementAttribute), true).Cast<BsonElementAttribute>().FirstOrDefault() }).Where(p => p.Attribute != null).ToDictionary(p => p.Property.Name, p => p.Attribute.ElementName);


        public static readonly string[] validNames = names.Keys.ToList().Concat(names.Values.ToList()).ToArray();
    }
    public static class AppUserOrders
    {
        public const int Id = 0;
        public const int Name = 1;
        public const int Email = 2;
        public const int Active = 3;
        public const int Permissions = 4;
        public const int LastAccess = 5;
        public const int ApiKey = 6;
    }
}
