using Application.Interfaces;
using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MealPlanService : IMealPlanService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        private readonly ISubstitutionRepository _substitutionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MealPlanService(
            IUserRepository userRepository,
            IWeeklyPlanRepository weeklyPlanRepository,
            ISubstitutionRepository substitutionRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _weeklyPlanRepository = weeklyPlanRepository;
            _substitutionRepository = substitutionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WeeklyPlan>> GetAvailablePlansForUserAsync(User user)
        {
            var plans = await _weeklyPlanRepository.GetAllAsync();

            var suitablePlans = plans
                .Where(p => PlanMatchesUser(user, p))
                .ToList();

            return suitablePlans;
        }

        private bool PlanMatchesUser(User user, WeeklyPlan plan)
        {
            /////////////////////////////////
            return true;
        }


        public async Task<bool> AssignPlanToUserAsync(Guid userId, Guid weeklyPlanId)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!user.Weight.HasValue || !user.Height.HasValue ||
                !user.ActivityLevel.HasValue || !user.Goal.HasValue)
            {
                return false;
            }

            var plan = await _weeklyPlanRepository.GetByIdAsync(weeklyPlanId);
            if (plan == null) return false;

            user.ClearDailyMealProgress();

            foreach (var dailyMeal in plan.DailyMeals)
            {
                var mealToUse = dailyMeal.Meal;

                var userAllergies = user.Allergies.ToList();
                if (mealToUse.ContainsAllergens(userAllergies))
                {
                    var substitution = await _substitutionRepository.GetByOriginalMealIdAsync(mealToUse.Id);
                    if (substitution != null)
                    {
                        mealToUse = substitution.SubstituteMeal;
                    }
                }

                var progress = new DailyMealProgress(
                    user.Id,
                    dailyMeal.Id,
                    mealToUse.Id != dailyMeal.Meal.Id ? mealToUse.Id : null
                );

                user.AddDailyMealProgress(progress);
            }

            user.SetWeeklyPlan(plan.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<List<WeeklyPlanPreviewVm>> BuildSelectablePlansAsync(User user)
        {
            var plans = await _weeklyPlanRepository.GetAllWithMealsAsync();
            var userAllergies = user.Allergies.ToList();

            var result = new List<WeeklyPlanPreviewVm>();

            foreach (var plan in plans)
            {
                var planVm = new WeeklyPlanPreviewVm
                {
                    PlanId = plan.Id,
                    Title = $"План от {plan.CreatedAt:dd.MM.yyyy}"
                };

                foreach (var dailyMeal in plan.DailyMeals)
                {
                    var mealToUse = dailyMeal.Meal;
                    bool substituted = false;

                    if (mealToUse.ContainsAllergens(userAllergies))
                    {
                        var substitution =
                            await _substitutionRepository.GetByOriginalMealIdAsync(mealToUse.Id);

                        if (substitution != null)
                        {
                            mealToUse = substitution.SubstituteMeal;
                            substituted = true;
                        }
                    }

                    planVm.Days.Add(new DailyMealPreviewVm
                    {
                        DayOfWeek = dailyMeal.DayIndex,
                        MealType = dailyMeal.MealType.ToString(),
                        MealName = mealToUse.Name,
                        IsSubstituted = substituted
                    });
                }

                result.Add(planVm);
            }

            return result;
        }


    }
}
