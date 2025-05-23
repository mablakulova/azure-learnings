namespace AzureBlobManager.WebApi.Logging.Settings;

public class AppInsightsSettings
{
    public bool Enabled { get; set; }
    public string? ConnectionString { get; set; }
}