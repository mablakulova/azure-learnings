using AzureBlobManager.Infrastructure.Common.Helpers;
using AzureBlobManager.WebApi.Logging.Helpers;
using AzureBlobManager.WebApi.Logging.Settings;
using Serilog;
using Serilog.Events;

namespace AzureBlobManager.WebApi.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder AddSerilogLogging(this IHostBuilder webBuilder)
    {
        return webBuilder.UseSerilog((context, loggerCfg) =>
        {
            loggerCfg
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("EnvironmentName", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithMachineName();

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerCfg
                   .WriteTo.Console()
                   .WriteTo.Debug();
            }
            else
            {
                loggerCfg
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .MinimumLevel.Override("System", LogEventLevel.Warning);
                
                var appInsightsSettings = context.Configuration.GetOptions<AppInsightsSettings>();
                if (appInsightsSettings.Enabled)
                {
                    loggerCfg.WriteTo.ApplicationInsights(
                        appInsightsSettings.ConnectionString,
                        TelemetryConverter.Traces,
                        restrictedToMinimumLevel: LogEventLevel.Error);
                }
            }
        });
    }
    
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder appBuilder)
    {
        return appBuilder
            .UseSerilogRequestLogging(
                opts => opts.GetLevel = LogHelper.ExcludeHealthChecks);
    }
}