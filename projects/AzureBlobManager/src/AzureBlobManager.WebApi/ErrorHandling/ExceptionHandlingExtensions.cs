using AzureBlobManager.WebApi.ErrorHandling.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobManager.WebApi.ErrorHandling;

public static class ExceptionHandlingExtensions
{
    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(o =>
        {
            if (o == null)
            {
                throw new ArgumentException($"Cannot find {nameof(MvcOptions)}");
            }

            o.Filters.Add<ExceptionHandlingFilter>();
        });
    }
}