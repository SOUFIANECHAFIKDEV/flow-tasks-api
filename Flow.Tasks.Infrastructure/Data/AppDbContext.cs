using Flow.Tasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flow.Tasks.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<UserEntity>(e =>
        {
            e.ToTable("Users");
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).HasMaxLength(200).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });

        b.Entity<TaskItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(256);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => !x.IsDeleted);

            e.HasOne(x => x.AssignedUser)
             .WithMany(u => u.Tasks)
             .HasForeignKey(x => x.AssignedUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
