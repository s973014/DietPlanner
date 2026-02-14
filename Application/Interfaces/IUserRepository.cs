using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(User user, CancellationToken cancellationToken = default);
        void Update(User user);
        void Remove(User user);
    }
}
