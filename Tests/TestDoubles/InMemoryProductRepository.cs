using Application.Interfaces;
using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestDoubles
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private readonly HashSet<Guid> _usedProducts = new();

        public void MarkAsUsed(Guid productId)
        {
            _usedProducts.Add(productId);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyList<Product>)_products);

        public Task<bool> IsUsedInAnyMealAsync(Guid productId, CancellationToken cancellationToken = default)
            => Task.FromResult(_usedProducts.Contains(productId));

        public Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public void Update(Product product) { }

        public void Remove(Product product)
        {
            _products.Remove(product);
        }
    }
}
