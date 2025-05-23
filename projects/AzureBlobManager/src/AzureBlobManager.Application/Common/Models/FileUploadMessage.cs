namespace AzureBlobManager.Application.Common.Models;

public class FileUploadMessage
{
    public Guid FileId { get; set; }
    public int UserId { get; set; }
    public string BlobName { get; set; }
    public string ContentType { get; set; }
}