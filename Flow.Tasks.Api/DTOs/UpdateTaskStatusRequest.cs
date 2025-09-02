namespace Flow.Tasks.Api.DTOs
{
    public sealed class UpdateTaskStatusRequest
    {
        public TaskStatus Status { get; set; }

        // On exige la rowversion actuelle pour gérer la concurrence optimiste
        public string RowVersion { get; set; } = default!;
    }
}
