namespace Flow.Tasks.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        // navigation inverse
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
