using Application.Interfaces;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly AppDbContext _context;

        public MealRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Meal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Meals
                .Include(m => m.Products)
                    .ThenInclude(mp => mp.Product)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Meal>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Meals
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Meal meal, CancellationToken cancellationToken = default)
        {
            await _context.Meals.AddAsync(meal, cancellationToken);
        }

        public void Update(Meal meal)
        {
            _context.Meals.Update(meal);
        }

        public void Remove(Meal meal)
        {
            _context.Meals.Remove(meal);
        }
    }
}
