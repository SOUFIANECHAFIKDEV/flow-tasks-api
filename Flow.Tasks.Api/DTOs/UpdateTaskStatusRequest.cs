namespace Flow.Tasks.Api.DTOs
{
    public sealed class UpdateTaskStatusRequest
    {
        public Domain.TaskStatus Status { get; set; }
        public string RowVersion { get; set; } = default!;
    }
}
