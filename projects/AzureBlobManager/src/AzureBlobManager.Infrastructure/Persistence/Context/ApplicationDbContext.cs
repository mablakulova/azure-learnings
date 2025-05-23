using Microsoft.EntityFrameworkCore;
using AzureBlobManager.Domain.Entities;
using AzureBlobManager.Domain.Common;
using AzureBlobManager.Application.Common.Dependencies.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AzureBlobManager.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly ICurrentUserService _currentUser;

    public DbSet<UserFile> UserFiles { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUser) 
       : base(options)
    {
        _currentUser = currentUser;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyEntityOverrides();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(UserFileInvariants.FileNameMaxLength);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(UserFileInvariants.ContentTypeMaxLength);
            entity.Property(e => e.Size).IsRequired();
            entity.Property(e => e.BlobName).IsRequired().HasMaxLength(UserFileInvariants.BlobNameMaxLength);
            entity.Property(e => e.BlobUri).IsRequired();
            entity.Property(e => e.UploadedAt).IsRequired();
            entity.Property(e => e.IsProcessed).IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Files)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private void ApplyEntityOverrides()
    {
        foreach (var entry in ChangeTracker.Entries<IAudited>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(IAudited.CreatedById)).CurrentValue = _currentUser.UserId;
                    entry.Property(nameof(IAudited.CreatedAt)).CurrentValue = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Property(nameof(IAudited.ModifiedById)).CurrentValue = _currentUser.UserId;
                    entry.Property(nameof(IAudited.ModifiedAt)).CurrentValue = DateTime.Now;
                    break;
            }
        }
    }
}