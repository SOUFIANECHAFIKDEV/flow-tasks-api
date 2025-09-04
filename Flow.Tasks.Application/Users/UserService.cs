using Flow.Tasks.Application.Abstractions;
using Flow.Tasks.Application.Tasks;
using Flow.Tasks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Tasks.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public Task<List<UserEntity>?> ListAsync(CancellationToken ct)
            => _repo.ListAsync(ct);

    }
}
