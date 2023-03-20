using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WeatherDataAPI.Attributes;

public class ApiKeyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes(typeof(ApiKeyAttribute), true).Any() || context.MethodInfo.DeclaringType.GetCustomAttributes(typeof(ApiKeyAttribute), true).Any())
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}
