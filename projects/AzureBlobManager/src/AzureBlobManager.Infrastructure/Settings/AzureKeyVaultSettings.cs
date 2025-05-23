namespace AzureBlobManager.Infrastructure.Settings;

public class AzureKeyVaultSettings
{
    public bool AddToConfiguration { get; init; }
    public string? ServiceUrl { get; init; }
}