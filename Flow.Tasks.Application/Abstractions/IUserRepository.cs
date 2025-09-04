using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<List<UserEntity>?> ListAsync(CancellationToken ct);
    }
}
