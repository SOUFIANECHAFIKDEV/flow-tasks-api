using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flow.Tasks.Api.Domain
{
    public sealed class TaskEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(120)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public string? AssignedTo { get; set; }
        [Timestamp]
        [Column(TypeName = "rowversion")]
        public byte[] RowVersion { get; set; } = default!;
    }
}
