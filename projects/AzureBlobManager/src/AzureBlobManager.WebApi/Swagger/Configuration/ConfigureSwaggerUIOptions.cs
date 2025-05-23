using AzureBlobManager.Infrastructure.Common.Helpers;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AzureBlobManager.WebApi.Swagger.Configuration;

internal class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerUIOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SwaggerUIOptions options)
    {
        var swaggerSettings = _configuration.GetOptions<SwaggerSettings>();

        options.SwaggerEndpoint(
            url: "/swagger/v1/swagger.json",
            name: swaggerSettings.ApiName + "v1"
        );
    }
}