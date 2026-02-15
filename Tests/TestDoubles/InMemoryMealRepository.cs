using Application.Interfaces;
using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestDoubles
{
    public class InMemoryMealRepository
    {
        private readonly List<Meal> _meals = new();

        public Task<Meal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(_meals.FirstOrDefault(m => m.Id == id));

        public Task<IReadOnlyList<Meal>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyList<Meal>)_meals);

        public Task AddAsync(Meal meal, CancellationToken cancellationToken = default)
        {
            _meals.Add(meal);
            return Task.CompletedTask;
        }

        public void Update(Meal meal) { }

        public void Remove(Meal meal)
        {
            _meals.Remove(meal);
        }
    }
}
