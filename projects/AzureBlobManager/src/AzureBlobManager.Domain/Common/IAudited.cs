namespace AzureBlobManager.Domain.Common;

public interface IAudited
{
    int CreatedById { get; }
    DateTime CreatedAt { get; }
    int? ModifiedById { get; }
    DateTime? ModifiedAt { get; }
}