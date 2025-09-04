using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Application.Users
{
    public interface IUserService
    {
        Task<List<UserEntity>?> ListAsync(CancellationToken ct);
    }
}
