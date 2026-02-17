using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWeeklyPlanRepository
    {
        Task<List<WeeklyPlan>> GetAllAsync();
        Task<WeeklyPlan?> GetByIdAsync(Guid id);
        Task AddAsync(WeeklyPlan plan);
        Task UpdateAsync(WeeklyPlan plan);
        Task DeleteAsync(Guid id);
        public IQueryable<WeeklyPlan> GetAllAsyncTracked();
        public Task<WeeklyPlan?> GetByIdWithMealsAsync(Guid id);
        public Task<List<WeeklyPlan>> GetAllWithMealsAsync();
    }

    public class WeeklyPlanPreviewVm
    {
        public Guid PlanId { get; set; }
        public string Title { get; set; }

        public List<DailyMealPreviewVm> Days { get; set; } = new();
    }

    public class DailyMealPreviewVm
    {
        public int DayOfWeek { get; set; }
        public string MealType { get; set; } 

        public string MealName { get; set; }
        public bool IsSubstituted { get; set; }
    }

}
