using Domain.Common;
using Domain.ValueObjects;
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

    public class DashboardReportVm
    {
        public string Title { get; set; } = "Подробный отчёт по недельному плану";

        public List<ReportDayVm> Days { get; set; } = new();

        public Nutrition PlannedTotal { get; set; } = new();
        public Nutrition ActualTotal { get; set; } = new();

        public int PlannedMeals { get; set; }
        public int EatenMeals { get; set; }

        public int CompletionPercent =>
            PlannedMeals == 0 ? 0 : (int)((double)EatenMeals / PlannedMeals * 100);
    }

    public class ReportDayVm
    {
        public int DayIndex { get; set; }
        public string DayName { get; set; }

        public List<ReportMealVm> Meals { get; set; } = new();

        public Nutrition Planned { get; set; } = new();
        public Nutrition Actual { get; set; } = new();
    }

    public class ReportMealVm
    {
        public string MealType { get; set; }
        public string MealName { get; set; }

        public bool IsEaten { get; set; }
        public bool IsSubstituted { get; set; }

        public Nutrition Planned { get; set; }
        public Nutrition Actual { get; set; }
    }

}
