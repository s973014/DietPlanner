using Application.Interfaces;
using Domain.Entitites;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "Dietitian")]
    public class WeeklyPlansController : Controller
    {
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WeeklyPlansController(
            IWeeklyPlanRepository weeklyPlanRepository,
            IMealRepository mealRepository,
            IUnitOfWork unitOfWork)
        {
            _weeklyPlanRepository = weeklyPlanRepository;
            _mealRepository = mealRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _weeklyPlanRepository.GetAllAsync();
            return View(plans);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var meals = await _mealRepository.GetAllAsync();

            var vm = new WeeklyPlanCreateVm
            {
                Meals = meals.Select(m => new MealSelectVm
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeeklyPlanCreateVm vm)
        {
            

            var plan = new WeeklyPlan();

            foreach (var dm in vm.DailyMeals)
            {
                var meal = await _mealRepository.GetByIdAsync(dm.MealId);
                if (meal == null) continue;

                plan.AddDailyMeal(new DailyMeal(
                    dm.DayIndex,
                    dm.MealType,
                    meal
                ));
            }

            await _weeklyPlanRepository.AddAsync(plan);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var plan = await _weeklyPlanRepository.GetByIdAsync(id);
            if (plan == null)
                return NotFound();

            var meals = await _mealRepository.GetAllAsync();

            var vm = new WeeklyPlanEditVm
            {
                Id = plan.Id,
                Meals = meals.Select(m => new MealSelectVm
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList(),

                DailyMeals = plan.DailyMeals.Select(dm => new DailyMealVm
                {
                    DayIndex = dm.DayIndex,
                    MealType = dm.MealType,
                    MealId = dm.Meal.Id
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WeeklyPlanEditVm vm)
        {
            
            var oldPlan = await _weeklyPlanRepository.GetByIdAsync(vm.Id);
            if (oldPlan == null) return NotFound();

           
            await _weeklyPlanRepository.DeleteAsync(vm.Id);
            await _unitOfWork.SaveChangesAsync();

            
            var newPlan = new WeeklyPlan() { Id = oldPlan.Id};

            foreach (var dm in vm.DailyMeals)
            {
                var meal = await _mealRepository.GetByIdAsync(dm.MealId);
                if (meal == null) continue;

                newPlan.AddDailyMeal(new DailyMeal(dm.DayIndex, dm.MealType, meal));
            }

            await _weeklyPlanRepository.AddAsync(newPlan);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        public async Task<IActionResult> Delete(Guid id)
        {
            var plan = await _weeklyPlanRepository.GetByIdAsync(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _weeklyPlanRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class WeeklyPlanCreateVm
    {
        public List<DailyMealVm> DailyMeals { get; set; } = new();
        public List<MealSelectVm> Meals { get; set; } = new();
    }

    public class WeeklyPlanEditVm
    {
        public Guid Id { get; set; }
        public List<DailyMealVm> DailyMeals { get; set; } = new();
        public List<MealSelectVm> Meals { get; set; } = new();
    }

    public class MealSelectVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class DailyMealVm
    {
        public int DayIndex { get; set; }
        public MealType MealType { get; set; }
        public Guid MealId { get; set; }
    }
}
