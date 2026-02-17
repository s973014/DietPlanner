using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class DailyMeal : Entity
    {
        public int DayIndex { get; private set; }
        public MealType MealType { get; private set; }

        public Guid MealId { get; private set; }
        public Meal Meal { get; private set; }

        public Guid WeeklyPlanId { get; private set; }
        public WeeklyPlan WeeklyPlan { get; private set; }

        private readonly List<DailyMealProgress> _progresses = new();
        public IReadOnlyCollection<DailyMealProgress> Progresses => _progresses;

        private DailyMeal() { }

        public DailyMeal(int dayIndex, MealType mealType, Meal meal)
        {
            Id = Guid.NewGuid();
            DayIndex = dayIndex;
            MealType = mealType;
            Meal = meal;
            MealId = meal.Id;
        }

        public void SetWeeklyPlan(WeeklyPlan plan)
        {
            WeeklyPlan = plan;
            WeeklyPlanId = plan.Id;
        }
    }
}
