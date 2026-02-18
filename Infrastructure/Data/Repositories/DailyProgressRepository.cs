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
    public class DailyMealProgressRepository : IDailyMealProgressRepository
    {
        private readonly AppDbContext _context;

        public DailyMealProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DailyMealProgress?> GetByIdAsync(Guid id)
        {
            return await _context.Progresses
                .Include(p => p.DailyMeal)
                    .ThenInclude(dm => dm.Meal)
                .Include(p => p.ReplacementMeal)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<DailyMealProgress>> GetByUserAndDayAsync(
            Guid userId,
            Guid weeklyPlanId,
            int dayOfWeek)
        {
            return await _context.Progresses
                .Include(p => p.DailyMeal)
                    .ThenInclude(dm => dm.Meal)
                .Include(p => p.ReplacementMeal)
                .Where(p =>
                    p.UserId == userId &&
                    p.DailyMeal.DayIndex == dayOfWeek &&
                    p.DailyMeal.WeeklyPlanId == weeklyPlanId)
                .OrderBy(p => p.DailyMeal.MealType)
                .ToListAsync();
        }

        public async Task<DailyMealProgress?> GetByUserAndDailyMealAsync(
            Guid userId,
            Guid dailyMealId)
        {
            return await _context.Progresses
                .Include(p => p.ReplacementMeal)
                .FirstOrDefaultAsync(p =>
                    p.UserId == userId &&
                    p.DailyMealId == dailyMealId);
        }

        public async Task AddAsync(DailyMealProgress progress)
        {
            _context.Progresses.Add(progress);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DailyMealProgress progress)
        {
            _context.Progresses.Update(progress);
            await _context.SaveChangesAsync();
        }

        public async Task SetEatenAsync(Guid userId, Guid dailyMealId, bool isEaten)
        {
            var progress = await _context.Progresses
                .FirstOrDefaultAsync(p =>
                    p.UserId == userId &&
                    p.DailyMealId == dailyMealId);

            if (progress == null)
            {
                progress = new DailyMealProgress(userId, dailyMealId);
                _context.Progresses.Add(progress);
            }

            if (isEaten)
                progress.MarkAsEaten();
            else
                progress.MarkAsNotEaten();

            await _context.SaveChangesAsync();
        }
    }
}
