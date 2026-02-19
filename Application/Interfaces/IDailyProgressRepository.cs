using Domain.Entitites;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces
{
    public interface IDailyMealProgressRepository
    {
        Task<DailyMealProgress?> GetByIdAsync(Guid id);

        Task<List<DailyMealProgress>> GetByUserAndDayAsync(
            Guid userId,
            Guid weeklyPlanId,
            int dayOfWeek);

        Task<DailyMealProgress?> GetByUserAndDailyMealAsync(
            Guid userId,
            Guid dailyMealId);

        Task AddAsync(DailyMealProgress progress);

        Task UpdateAsync(DailyMealProgress progress);
        public Task SetEatenAsync(Guid userId, Guid dailyMealId, bool isEaten);

        Task<DashboardReportVm> BuildFullReportAsync(Guid userId);

        public Task ClearWeeklyPlanProgressAsync(Guid userId);
    }

    
}
