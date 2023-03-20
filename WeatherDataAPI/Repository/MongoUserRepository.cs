using Amazon.Runtime.EventStreams.Internal;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using WeatherDataAPI.Models.AppUser;
using WeatherDataAPI.Models.User;
using WeatherDataAPI.Models.User.DTO;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Services;

namespace WeatherDataAPI.Repository
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<AppUser> _users;
        public MongoUserRepository(MongoConnection connection)
        {
            _users = connection.GetDatabase().GetCollection<AppUser>("Users");
        }
        public AppUser AuthenticateUser(string ApiKey, List<string> requiredAccessLevel)
        {
            //find user by apikey
            var filter = Builders<AppUser>.Filter.Eq(c => c.ApiKey, ApiKey);
            var user = _users.Find(filter).FirstOrDefault();
            foreach (var permission in requiredAccessLevel)
            {
                if (user == null || !IsSuitableRole(user.Permissions, permission) || !user.Active.Value)
                {
                    //user not found or role not suitable
                    return null;
                }
            }
            return user;
        }

        private bool IsSuitableRole(Permissions permissions, string requiredPermission)
        {
            //check if a role has the required access level
            if (permissions == null)
            {
                return false;
            }
            return permissions.checkPermission(requiredPermission);
        }

        public string CreateUser(AppUserBase user)
        {
            //check if the user already exists
            var filter = Builders<AppUser>.Filter.Eq(c => (c.Email), user.Email);
            var existingUser = _users.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                //user already exists
                return null;
            }
            var newUser = new AppUser
            {
                Name = user.Name,
                Email = user.Email,
                Active = user.Active,
                Permissions = user.Permissions,
                LastAccess = DateTime.Now,
                ApiKey = Guid.NewGuid().ToString(),

            };
            _users.InsertOne(newUser);

            return newUser._id.ToString();
        }
        
        public List<AppUser> CreateUsers(List<AppUserBase> users)
        {
            var filter = Builders<AppUser>.Filter.In(AppUserNames.Email, users.Select(user => user.Email));
            var existingUser = _users.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                //user already exists
                //reaise an error that a user already exists
                
                throw new Exception("A user with email '" + existingUser.Email + "' already exists." );
            }
            var newUsers = users.Select(user => new AppUser
            {
                
                Name = user.Name,
                Email = user.Email,
                Active = user.Active,
                Permissions = user.Permissions
                
            }).ToList();
            _users.InsertMany(newUsers);
            return newUsers;
        }

        public void UpdateLoginTime(string ApiKey, DateTime loginData)
        {
            //update the last access time of the user
            var filter = Builders<AppUser>.Filter.Eq(c => c.ApiKey, ApiKey);
            var update = Builders<AppUser>.Update.Set(c => c.LastAccess, loginData);
            var result = _users.UpdateOne(filter, update);

        }

        public AppUser GetUser(string id)
        {
            var filter = Builders<AppUser>.Filter.Eq(c => c._id, new ObjectId(id));
            var result = _users.Find(filter).FirstOrDefault();
            return result;
        }

        public List<AppUser> GetUsers(AppUserFilter filter)
        {
            return _users.FindSync(filter.BuildFilter(),filter.BuildFindOptions()).ToList();
            
        }

        public long UpdateUser(AppUserBase user, string id)
        {
            if (user.Email != null)
            {
                var filter = Builders<AppUser>.Filter.Eq(c => (c.Email), user.Email);
                var existingUser = _users.Find(filter).FirstOrDefault();
                if (existingUser != null)
                {
                    //user already exists
                    throw new Exception("A user with email '" + existingUser.Email + "' already exists.");
                }
            } 

            var update = user.ToUpdateDefinition();
            //if update equals null then no valid fields were provided
            if (update == null)
            {
                return -1; // bad fields
            }
            var result = _users.UpdateOne(user => user._id == new ObjectId(id), update);

            if (result.MatchedCount > 0)
            {
                return 1; // worked
            }
            else
            {
                return 0; // nothing matched

            }
        }

        public long UpdateUsers(List<AppUserUpdateManyDTO> users)
        {
            var filter = Builders<AppUser>.Filter.In(AppUserNames.Email, users.Select(user => user.Update.Email));
            var existingUser = _users.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                //user already exists
                //reaise an error that a user already exists

                throw new Exception("A user with email '" + existingUser.Email + "' already exists.");
            }
            //speedy bulk write magic
            //we need to create a list of models as each model will define its own filter and update definition
            var updates = new UpdateManyModel<AppUser>[users.Count];
            for (int index = 0; index < users.Count; index++)
            {
                var update = users[index].Update.ToUpdateDefinition();
                var model = new UpdateManyModel<AppUser>(Builders<AppUser>.Filter.Eq("_id", users[index]._id), update);
                updates[index] = model;
            }
            //do the bulk write / bulk update using the created models
            var result = _users.BulkWrite(updates);
            return result.MatchedCount;
        }

        public string ResetApiKey(string id)
        {
            //update the api key to a new key
            var newKey = Guid.NewGuid().ToString();
            var filter = Builders<AppUser>.Filter.Eq(c => c._id, new ObjectId(id));
            var update = Builders<AppUser>.Update.Set(c => c.ApiKey, newKey);
            var result = _users.UpdateOne(filter, update);
            if (result.MatchedCount > 0)
            {
                return newKey;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteUser(string id)
        {
            return _users.DeleteOne(user => user._id == new ObjectId(id)).DeletedCount >= 1;
        }

        public long DeleteUsers(AppUserFilter filter)
        {
            //get all ids to delete
            var ids = GetUsers(filter).Select(x => x._id);
            //delete the ids
            return _users.DeleteMany(user => ids.Contains(user._id)).DeletedCount;
        }

        public bool ReplaceUser(AppUser user)
        {
            var filter = Builders<AppUser>.Filter.Eq(c => (c.Email), user.Email);
            var existingUser = _users.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                //user already exists
                throw new Exception("A user with email '" + existingUser.Email + "' already exists.");
            }
            var result = _users.ReplaceOne(appUser => appUser._id == user._id, user);
            return result.ModifiedCount >= 1;
        }

        public long ReplaceUsers(List<AppUserCreateManyDTO> users)
        {
            var filter = Builders<AppUser>.Filter.In(AppUserNames.Email, users.Select(user => user.Email));
            var existingUser = _users.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                //user already exists
                //reaise an error that a user already exists

                throw new Exception("A user with email '" + existingUser.Email + "' already exists.");
            }
            var result = _users.BulkWrite(users.Select(appUser => new ReplaceOneModel<AppUser>(new BsonDocument("_id", appUser._id), appUser.ToAppUser())));
            return result.ModifiedCount;
        }
    }
}
