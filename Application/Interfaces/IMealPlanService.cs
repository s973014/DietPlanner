using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMealPlanService
    {
        Task<List<WeeklyPlan>> GetAvailablePlansForUserAsync(User user);
        Task<bool> AssignPlanToUserAsync(Guid userId, Guid weeklyPlanId);
        public Task<List<WeeklyPlanPreviewVm>> BuildSelectablePlansAsync(User user);
    }
}
