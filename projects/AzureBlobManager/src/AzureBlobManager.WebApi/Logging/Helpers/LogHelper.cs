using Serilog.Events;

namespace AzureBlobManager.WebApi.Logging.Helpers;

public static class LogHelper
{
    public static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
        ex != null
            ? LogEventLevel.Error
            : ctx.Response.StatusCode > 499
                ? LogEventLevel.Error
                : IsHealthCheckEndpoint(ctx)
                    ? LogEventLevel.Verbose
                    : LogEventLevel.Information;

    private static bool IsHealthCheckEndpoint(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();
        if (endpoint is object)
        {
            return string.Equals(
                endpoint.DisplayName,
                "Health checks",
                StringComparison.Ordinal);
        }
        
        return false;
    }
}