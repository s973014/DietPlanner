using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class WeeklyPlan : Entity
    {
        public DateTime CreatedAt { get; private set; }

        private readonly List<DailyMeal> _dailyMeals = new();
        public IReadOnlyCollection<DailyMeal> DailyMeals => _dailyMeals;

        public WeeklyPlan()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public void AddDailyMeal(DailyMeal meal)
        {
            meal.SetWeeklyPlan(this);
            _dailyMeals.Add(meal);
        }

        public void ClearDailyMeals()
        {
            _dailyMeals.Clear();
        }
        public void RemoveDailyMeal(DailyMeal meal)
        {
            _dailyMeals.Remove(meal);
        }
        public void UpdateCreatedAt(DateTime newTime)
        {
            CreatedAt = newTime;
        }
    }
}
