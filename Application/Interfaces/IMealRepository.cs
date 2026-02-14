using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMealRepository
    {
        Task<Meal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Meal>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Meal meal, CancellationToken cancellationToken = default);
        void Update(Meal meal);
        void Remove(Meal meal);
    }
}
