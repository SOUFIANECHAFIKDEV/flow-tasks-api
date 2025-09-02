using Flow.Tasks.Api.App.Domain;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Flow.Tasks.Api.App.Domain.TaskStatus;

namespace Flow.Tasks.Api.Data
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(120);
                b.Property(x => x.Status).HasConversion<int>();
                b.Property(x => x.RowVersion).IsRowVersion(); // important pour SQL Server

                // Seed minimal (optionnel)
                var t1 = new TaskItem { Id = Guid.NewGuid(), Title = "Évaluer patient A", Status = TaskStatus.Todo };
                var t2 = new TaskItem { Id = Guid.NewGuid(), Title = "Rédiger compte rendu", Status = TaskStatus.InProgress };
                b.HasData(t1, t2);
            });
        }
    }
}
