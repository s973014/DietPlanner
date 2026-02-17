using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class DailyMealProgress : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid DailyMealId { get; set; }
        public DailyMeal DailyMeal { get; set; }

        public bool IsEaten { get;  set; } = false;

        public Guid? ReplacementMealId { get; set; }
        public Meal? ReplacementMeal { get; set; }

        private DailyMealProgress() { }

        public DailyMealProgress(Guid userId, Guid dailyMealId, Guid? replacementMealId = null)
        {
            UserId = userId;
            DailyMealId = dailyMealId;
            ReplacementMealId = replacementMealId;
        }

        public void MarkAsEaten() => IsEaten = true;
        public void MarkAsNotEaten() => IsEaten = false;

        public void SetReplacement(Guid replacementMealId)
        {
            ReplacementMealId = replacementMealId;
        }
    }

}
