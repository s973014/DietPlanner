using Application.Interfaces;
using Domain.Entitites;
using Domain.ValueObjects;
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

        public async Task ClearWeeklyPlanProgressAsync(Guid userId)
        {
            var progresses = await _context.Progresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.Progresses.RemoveRange(progresses);
            await _context.SaveChangesAsync();
        }


        public async Task<DashboardReportVm> BuildFullReportAsync(Guid userId)
        {
            var progresses = await _context.Progresses  
                .Include(p => p.DailyMeal)
                    .ThenInclude(dm => dm.Meal)
                        .ThenInclude(m => m.Products)
                            .ThenInclude(mp => mp.Product)
                .Include(p => p.ReplacementMeal)
                    .ThenInclude(m => m.Products)
                        .ThenInclude(mp => mp.Product)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var report = new DashboardReportVm();

            var groupedByDay = progresses
                .GroupBy(p => p.DailyMeal.DayIndex)
                .OrderBy(g => g.Key);

            foreach (var dayGroup in groupedByDay)
            {
                var dayVm = new ReportDayVm
                {
                    DayIndex = dayGroup.Key,
                    DayName = GetDayName(dayGroup.Key)
                };

                foreach (var p in dayGroup)
                {
                    var baseMeal = p.DailyMeal.Meal;
                    var plannedMeal = p.ReplacementMeal ?? baseMeal;
                    var actualMeal = p.IsEaten ? plannedMeal : null;

                    var plannedNutrition = CalculateNutrition(plannedMeal);
                    var actualNutrition = p.IsEaten
                        ? plannedNutrition
                        : new Nutrition();

                    var mealVm = new ReportMealVm
                    {
                        MealType = p.DailyMeal.MealType.ToString(),
                        MealName = plannedMeal.Name,
                        IsEaten = p.IsEaten,
                        IsSubstituted = p.ReplacementMealId != null,
                        Planned = plannedNutrition,
                        Actual = actualNutrition
                    };

                    dayVm.Planned += plannedNutrition;
                    dayVm.Actual += actualNutrition;

                    report.PlannedTotal += plannedNutrition;
                    report.ActualTotal += actualNutrition;

                    report.PlannedMeals++;
                    if (p.IsEaten) report.EatenMeals++;

                    dayVm.Meals.Add(mealVm);
                }

                report.Days.Add(dayVm);
            }

            return report;
        }

        private Nutrition CalculateNutrition(Meal meal)
        {
            var result = new Nutrition();

            foreach (var mp in meal.Products)
            {
                var product = mp.Product;
                var weightFactor = mp.AmountInGrams / 100f;

                var n = mp.CalculateNutrition();

                result.Calories += n.Calories * weightFactor;
                result.Proteins += n.Proteins * weightFactor;
                result.Fats += n.Fats * weightFactor;
                result.Carbs += n.Carbs * weightFactor;
            }

            return result;
        }

        private static string GetDayName(int day) => day switch
        {
            0 => "Понедельник",
            1 => "Вторник",
            2 => "Среда",
            3 => "Четверг",
            4 => "Пятница",
            5 => "Суббота",
            6 => "Воскресенье",
            _ => ""
        };

    }
}
