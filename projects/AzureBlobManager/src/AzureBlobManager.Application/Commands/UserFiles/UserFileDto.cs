using AzureBlobManager.Application.Common.Mapping;
using AzureBlobManager.Domain.Entities;

namespace AzureBlobManager.Application.Commands;

public class UserFileDto : IMapFrom<UserFile>
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string BlobUri { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsProcessed { get; set; }
}