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
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsCompleted { get; private set; }

        private readonly List<DailyMeal> _dailyMeals = new();
        public IReadOnlyCollection<DailyMeal> DailyMeals => _dailyMeals;

        private WeeklyPlan() { }
        public WeeklyPlan(User user)
        {
            Id = Guid.NewGuid();
            User = user;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddDailyMeal(DailyMeal meal)
        {
            _dailyMeals.Add(meal);
        }

        public void Complete()
        {
            IsCompleted = true;
        }
    }
}
