using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security;
using WeatherDataAPI.Models.AppUser;
using WeatherDataAPI.Repository;

namespace WeatherDataAPI.Attributes
{
   
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute
    {
        private List<string> requiredPermissions;
        public List<string> RequiredPermissions { get => requiredPermissions; }

        public ApiKeyAttribute(string permission = nameof(Permissions.Admin))
        {
            requiredPermissions = new List<string> { permission };
        }
        public ApiKeyAttribute(List<string> permissions)
        {
            if (permissions == null)
            {
                requiredPermissions = new List<string> { nameof(Permissions.Admin) };

            }
            requiredPermissions = new List<string>(permissions);

        }
        public ApiKeyAttribute(params string[] permissions) {
            if (permissions == null || permissions.Length <=0)
            {
                requiredPermissions = new List<string> { nameof(Permissions.Admin) };

            }
            requiredPermissions = new List<string>(permissions);
        }

    }

}
