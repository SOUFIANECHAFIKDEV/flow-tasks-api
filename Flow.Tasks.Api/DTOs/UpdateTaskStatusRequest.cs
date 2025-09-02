namespace Flow.Tasks.Api.DTOs
{
    public sealed class UpdateTaskStatusRequest
    {
        public TaskStatus Status { get; set; }
        public string RowVersion { get; set; } = default!;
    }
}
