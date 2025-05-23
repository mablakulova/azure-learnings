using System.Reflection;
using AzureBlobManager.Application;
using AzureBlobManager.Infrastructure;
using AzureBlobManager.WebApi.API;
using AzureBlobManager.WebApi.Authentication;
using AzureBlobManager.WebApi.CORS;
using AzureBlobManager.WebApi.ErrorHandling;
using AzureBlobManager.WebApi.Logging;
using AzureBlobManager.WebApi.Swagger;
using AzureBlobManager.WebApi.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .AddSerilogLogging()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddUserSecrets(Assembly.GetEntryAssembly(), optional: true);
        config.AddInfrastructureConfiguration();
    });

builder.Services.AddApi();
builder.Services.AddAuthUserServices();
builder.Services.AddExceptionHandling();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddMyVersioning();
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();

var app = builder.Build();

app.UseRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCorsConfiguration();
app.UseSwagger(builder.Configuration);
app.UseInfrastructure(builder.Configuration);
app.UseApi();

app.Run();