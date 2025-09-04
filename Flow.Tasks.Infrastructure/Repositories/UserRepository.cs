using Flow.Tasks.Application;
using Flow.Tasks.Application.Abstractions;
using Flow.Tasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flow.Tasks.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Data.AppDbContext _db;
    public UserRepository(Data.AppDbContext db) => _db = db;

    public async Task<List<UserEntity>?> ListAsync(CancellationToken ct)
    {
        var list = await _db.Users.Where(t => !t.IsDeleted).ToListAsync(ct);

        return list;
    }
}
