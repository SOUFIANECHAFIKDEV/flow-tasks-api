using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flow.Tasks.Api.Domain
{
    public sealed class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(120)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        // Pour la démo, on garde un simple texte (email/username)
        public string? AssignedTo { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // SQL Server: colonne ROWVERSION / timestamp -> byte[] + [Timestamp]
        [Timestamp]
        [Column(TypeName = "rowversion")]
        public byte[] RowVersion { get; set; } = default!;
    }
}
