namespace AzureBlobManager.Domain.Entities;

public class UserFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string BlobName { get; set; }
    public string BlobUri { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsProcessed { get; set; }
    public string? Hash { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}