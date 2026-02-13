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
        public Meal Meal { get; private set; }
        public bool IsEaten { get; private set; }

        private DailyMeal() { }

        public DailyMeal(int dayIndex, MealType mealType, Meal meal)
        {
            Id = Guid.NewGuid();
            DayIndex = dayIndex;
            MealType = mealType;
            Meal = meal;
        }

        public void MarkAsEaten()
        {
            IsEaten = true;
        }
    }
}
