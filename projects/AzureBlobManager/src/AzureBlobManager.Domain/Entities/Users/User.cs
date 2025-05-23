using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace AzureBlobManager.Domain.Entities;

public class User : IdentityUser<int>
{
    public Collection<UserFile> Files { get; set; } = new();
}