using WeatherDataAPI.Models.User;
using WeatherDataAPI.Models.User.DTO;

namespace WeatherDataAPI.Repository
{
    public interface IUserRepository
    {
        AppUser AuthenticateUser(string ApiKey, List<string> requiredAccessLevel);
        string CreateUser(AppUserBase user);
        List<AppUser> CreateUsers(List<AppUserBase> users);
        void UpdateLoginTime(string ApiKey, DateTime loginData);
        AppUser GetUser(string id);
        List<AppUser> GetUsers(AppUserFilter filter);
        bool ReplaceUser(AppUser user);
        long ReplaceUsers(List<AppUserCreateManyDTO> users);
        long UpdateUser(AppUserBase user, string id);
        long UpdateUsers(List<AppUserUpdateManyDTO> users);
        string ResetApiKey(string id);
        bool DeleteUser(string id);
        long DeleteUsers(AppUserFilter filter);



    }
}
