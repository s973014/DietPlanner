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
    public class WeeklyPlanRepository : IWeeklyPlanRepository
    {
        private readonly AppDbContext _context;

        public WeeklyPlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeeklyPlan>> GetAllAsync()
        {
            return await _context.WeeklyPlans
                .Include(w => w.DailyMeals)
                .ThenInclude(dm => dm.Meal)
                .ToListAsync();
        }

        public async Task<WeeklyPlan?> GetByIdAsync(Guid id)
        {
            return await _context.WeeklyPlans
                .Include(w => w.DailyMeals)
                    .ThenInclude(dm => dm.Meal)
                        .ThenInclude(m => m.Products)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.NutritionPer100g)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(WeeklyPlan plan)
        {
            await _context.WeeklyPlans.AddAsync(plan);
        }

        public async Task UpdateAsync(WeeklyPlan plan)
        {
            _context.WeeklyPlans.Update(plan);
        }

        public async Task DeleteAsync(Guid id)
        {
            var plan = await GetByIdAsync(id);
            if (plan != null)
                _context.WeeklyPlans.Remove(plan);
        }

        public IQueryable<WeeklyPlan> GetAllAsyncTracked()
        {
            return _context.WeeklyPlans.AsQueryable();
        }

        public async Task<WeeklyPlan?> GetByIdWithMealsAsync(Guid id)
        {
    return await _context.WeeklyPlans
        .Include(w => w.DailyMeals)
        .ThenInclude(dm => dm.Meal)
        .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<WeeklyPlan>> GetAllWithMealsAsync()
        {
            return await _context.WeeklyPlans
                .Include(wp => wp.DailyMeals)
                    .ThenInclude(dm => dm.Meal)
                        .ThenInclude(m => m.Products)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.NutritionPer100g)
                .Include(wp => wp.DailyMeals)
                    .ThenInclude(dm => dm.Meal)
                        .ThenInclude(m => m.Products)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.Allergy)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<WeeklyPlan?> GetByIdWithDailyMealsAsync(Guid id)
        {
            return await _context.WeeklyPlans
                .Include(wp => wp.DailyMeals)
                    .ThenInclude(dm => dm.Meal)
                        .ThenInclude(m => m.Products)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.NutritionPer100g)
                .FirstOrDefaultAsync(wp => wp.Id == id);
        }

    }
}
