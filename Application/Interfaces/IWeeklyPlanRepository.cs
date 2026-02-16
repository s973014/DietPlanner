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
    }
}
