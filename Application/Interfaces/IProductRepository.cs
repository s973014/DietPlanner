using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<bool> IsUsedInAnyMealAsync(Guid productId, CancellationToken cancellationToken = default);

        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        void Update(Product product);
        void Remove(Product product);
    }
}
