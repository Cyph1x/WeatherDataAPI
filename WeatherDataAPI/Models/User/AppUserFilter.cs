using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using WeatherDataAPI.Models.AppUser;

namespace WeatherDataAPI.Models.User
{
    public class AppUserFilter
    {
        [JsonPropertyName(AppUserNames.Name)]
        [JsonPropertyOrder(AppUserOrders.Name)]
        public string? Name { get; set; }
        [JsonPropertyName(AppUserNames.Email)]
        [JsonPropertyOrder(AppUserOrders.Email)]

        public string? Email { get; set; }
        [JsonPropertyName(AppUserNames.Active)]
        [JsonPropertyOrder(AppUserOrders.Active)]
        public bool? Active { get; set; }
        [JsonPropertyName(AppUserNames.Permissions)]
        [JsonPropertyOrder(AppUserOrders.Permissions)]
        public Permissions? Permissions { get; set; }
        /// <summary>
        /// Oldest time
        /// </summary>

        [JsonPropertyName(AppUserNames.LastAccess + " start")]
        [JsonPropertyOrder(AppUserOrders.LastAccess)]
        public DateTime? LastAccessedStart { get; set; }
        /// <summary>
        /// Newest Time
        /// </summary>
        [JsonPropertyName(AppUserNames.LastAccess + " end")]
        [JsonPropertyOrder(AppUserOrders.LastAccess)]
        public DateTime? LastAccessedEnd { get; set; }
        [JsonPropertyName(AppUserNames.ApiKey)]
        [JsonPropertyOrder(AppUserOrders.ApiKey)]
        public string? ApiKey { get; set; }
        [JsonPropertyName(AppUserNames.ApiKey + "s")]
        [JsonPropertyOrder(AppUserOrders.ApiKey)]
        public List<string>? ApiKeys { get; set; }
        [JsonPropertyName(AppUserNames.Id)]
        [JsonPropertyOrder(AppUserOrders.Id)]
        public string? Id { get; set; }
        [JsonPropertyName(AppUserNames.Id + "s")]
        [JsonPropertyOrder(AppUserOrders.Id)]
        public List<string>? Ids { get; set; }
        /// <summary>
        /// Max amount to return
        /// </summary>
        public int? Limit { get; set; } = 100;
        /// <summary>
        /// amount of results to skip
        /// </summary>
        public int? Skip { get; set; }

        internal FilterDefinition<AppUser> BuildFilter()
        {
            FilterDefinitionBuilder<AppUser> builder = Builders<AppUser>.Filter;
            FilterDefinition<AppUser> filter = builder.Empty;
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
            if (!string.IsNullOrEmpty(ApiKey))
            {
                filter &= builder.Eq(AppUserNames.ApiKey, ApiKey);
            }
            if (ApiKeys != null)
            {
                if (ApiKeys.Count > 0)
                {
                    filter &= builder.In(AppUserNames.ApiKey, ApiKeys);
                }
            }
            if (!string.IsNullOrEmpty(Name))
            {

                filter &= builder.Eq(AppUserNames.Name, Name);

            }
            if (!string.IsNullOrEmpty(Email))
            {
                filter &= builder.Eq(AppUserNames.Email, Email);
            }
            if (Active != null)
            {
                filter &= builder.Eq(AppUserNames.Active, Active);
            }
            if (LastAccessedStart != null)
            {
                filter &= builder.Gt(AppUserNames.LastAccess, LastAccessedStart);
            }
            if (LastAccessedEnd != null)
            {
                filter &= builder.Lt(AppUserNames.LastAccess, LastAccessedEnd);
            }
            if (Permissions != null)
            {
                // for each defined permission in permissions
                var type = typeof(Permissions);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var name = property.Name;
                    if (property.GetValue(Permissions) != null)
                    {
                        filter &= builder.Eq(AppUserNames.Permissions + "." + PermissionsNames.names[name], (bool)property.GetValue(Permissions));
                    }
                }



            }
            return filter;
        }
        internal FindOptions<AppUser> BuildFindOptions()
        {
            return new FindOptions<AppUser>
            {
                Limit = Limit,
                Skip = Skip
            };
        }
    }
}
