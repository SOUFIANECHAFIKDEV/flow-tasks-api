namespace Flow.Tasks.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Todo;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public int? AssignedUserId { get; set; }        // FK nullable
        public UserEntity? AssignedUser { get; set; }         // navigation
    }
}
