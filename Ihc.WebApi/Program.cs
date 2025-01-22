using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetSection(ControllerConfiguration.Name).Get<ControllerConfiguration>();

// Exit if no configuration is present
if (config == null)
    return;

builder.Services.AddLogging();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IControllerConfiguration>(config);

// Add services to the container.
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IAccessControlService, AccessControlService>();
builder.Services.AddScoped<IAuthCacheService, AuthCacheService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProblemService, ProblemService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IXmlService, XmlService>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<ISoapDateService, SoapDateService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
 {
     x.SwaggerDoc(
         "v1",
         new OpenApiInfo
         {
             Title = $"{Assembly.GetExecutingAssembly().GetName().Name}",
             Version = "Version 1",
             Description = "Create documentation for myApp",
         });
     var xmlFilename = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
     x.IncludeXmlComments(xmlFilename);
 });

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
