
namespace AzureBlobManager.Domain.Common;

public abstract class Entity : IAudited
{
    public int CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int? ModifiedById { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
}