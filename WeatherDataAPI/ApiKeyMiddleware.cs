using WeatherDataAPI.Attributes;
using WeatherDataAPI.Repository;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;
    private readonly string _role;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requiresApiKeyAuth = context.GetEndpoint()?.Metadata.GetMetadata<ApiKeyAttribute>() != null;



        if (requiresApiKeyAuth)
        {
            //get the permissions
            var requiredPermissions = context.GetEndpoint()?.Metadata.GetMetadata<ApiKeyAttribute>()?.RequiredPermissions;
            //get the api key
            if (!context.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Api Key not found or Authorization header not present.");
                return;

            }
            var betterKey = apiKey.ToString().Trim('{', '}');
            if (!Guid.TryParse(betterKey, out _))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid Api Key.");
                return;
            }
            var userRepo = context.RequestServices.GetRequiredService<IUserRepository>();

            if (userRepo.AuthenticateUser(betterKey, requiredPermissions) == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Api Key does not exist, or has insufficient access.");
                return;
            }

        }

        await _next(context);
    }
}