using FluentValidation;
using FluentValidation.Validators;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using WeatherDataAPI;
using WeatherDataAPI.Models.User;
using WeatherDataAPI.Models.User.DTO;
using WeatherDataAPI.Models.User.Validation;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models.WeatherReadings.DTO;
using WeatherDataAPI.Models.WeatherReadings.Validation;
using WeatherDataAPI.Repository;
using WeatherDataAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin();
            policy.WithMethods("GET", "POST", "PUT", "DELETE");
        });

});
// Add services to the container.
services.Configure<DefaultMongoConnection>(builder.Configuration.GetSection("DefaultMongoConnection"));
services.AddScoped<MongoConnection>();
//repos
services.AddScoped<IReadingRepository, MongoReadingRepository>();
services.AddScoped<IUserRepository, MongoUserRepository>();
//Weather Reading validators
services.AddScoped<IValidator<WeatherReadingBase>, WeatherReadingBaseValidator>();
services.AddScoped<IValidator<List<WeatherReadingBase>>, WeatherReadingBaseManyValidator>();
services.AddScoped<IValidator<WeatherReadingUpdateDTO>, WeatherReadingUpdateDTOValidator>();
services.AddScoped<IValidator<List<WeatherReadingUpdateManyDTO>>, WeatherReadingUpdateManyDTOManyValidator>();
services.AddScoped<IValidator<WeatherReading>, WeatherReadingValidator>();
services.AddScoped<IValidator<List<WeatherReading>>, WeatherReadingManyValidator>();
services.AddScoped<IValidator<WeatherReadingFilter>, WeatherReadingFilterValidator>();
services.AddScoped<IValidator<List<WeatherReadingFilter>>, WeatherReadingFilterManyValidator>();
//App user validators
services.AddScoped<IValidator<AppUserBase>, AppUserBaseValidator>();
services.AddScoped<IValidator<List<AppUserBase>>, AppUserBaseManyValidator>();
services.AddScoped<IValidator<AppUserCreateManyDTO>, AppUserCreateManyDTOValidator>();
services.AddScoped<IValidator<List<AppUserCreateManyDTO>>, AppUserCreateManyDTOManyValidator>();
services.AddScoped<IValidator<AppUserUpdateManyDTO>,AppUserUpdateManyDTOValidator>();
services.AddScoped<IValidator<List<AppUserUpdateManyDTO>>, AppUserUpdateManyDTOManyValidator>();
services.AddScoped<IValidator<AppUserFilter>, AppUserFilterValidator>();



services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "API key needed to access the endpoints"
    });
    options.OperationFilter<ApiKeyOperationFilter>();
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
// Add FV
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
// Add FV validators
services.AddValidatorsFromAssemblyContaining<Program>();
// Add FV Rules to swagger
services.AddFluentValidationRulesToSwagger();
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var app = builder.Build();
app.UseCors();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();


#if DEBUG
  app.Use(async (context, next) =>
    {
        var bodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            await next();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            Console.WriteLine($"Response Body: {responseBodyText}");
            await responseBody.CopyToAsync(bodyStream);
        }
    });
#else
// Release work here  
#endif
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
