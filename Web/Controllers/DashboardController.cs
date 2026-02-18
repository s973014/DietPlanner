using Application.Interfaces;
using Domain.Entitites;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{

    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        private readonly IDailyMealProgressRepository _progressRepository;

        public DashboardController(
            IUserRepository userRepository,
            IWeeklyPlanRepository weeklyPlanRepository,
            IDailyMealProgressRepository progressRepository)
        {
            _userRepository = userRepository;
            _weeklyPlanRepository = weeklyPlanRepository;
            _progressRepository = progressRepository;
        }

        public async Task<IActionResult> Index(int day = 0)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var user = await _userRepository.GetByIdAsync(userId);

            ViewData["UserName"] = user.Name;
            ViewData["UserRole"] = user.Role.ToString();
            ViewData["UserWeight"] = user.Weight;
            ViewData["UserHeight"] = user.Height;
            ViewData["UserActivityLevel"] = user.ActivityLevel;
            ViewData["UserGoal"] = user.Goal;

            if (user.WeeklyPlanId == null)
                return View();

            var plan = await _weeklyPlanRepository
                .GetByIdWithDailyMealsAsync(user.WeeklyPlanId.Value);

            var dailyMeals = plan.DailyMeals
                .Where(dm => dm.DayIndex == day)
                .OrderBy(dm => dm.MealType)
                .ToList();

            var progress = await _progressRepository
                .GetByUserAndDayAsync(user.Id, plan.Id, day);

            var vm = new DashboardPlanVm
            {
                WeeklyPlanId = plan.Id,
                DayIndex = day,
                DayName = GetDayName(day),
                Meals = dailyMeals.Select(dm =>
                {
                    var p = progress.FirstOrDefault(x => x.DailyMealId == dm.Id);

                    return new DashboardMealVm
                    {
                        DailyMealId = dm.Id,
                        MealType = dm.MealType.ToString(),
                        MealName = p?.ReplacementMeal?.Name ?? dm.Meal.Name,
                        IsEaten = p?.IsEaten ?? false,
                        IsSubstituted = p?.ReplacementMealId != null
                    };
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMeal([FromBody] ToggleMealRequest request)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            await _progressRepository.SetEatenAsync(userId, request.DailyMealId, request.IsEaten);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FinishPlan()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);


            await _userRepository.ClearWeeklyPlanAsync(userId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }

        private static string GetDayName(int day) => day switch
        {
            0 => "Понедельник",
            1 => "Вторник",
            2 => "Среда",
            3 => "Четверг",
            4 => "Пятница",
            5 => "Суббота",
            6 => "Воскресенье",
            _ => ""
        };
    }

    public class DashboardPlanVm
    {
        public Guid WeeklyPlanId { get; set; }

        public int DayIndex { get; set; }
        public string DayName { get; set; }

        public List<DashboardMealVm> Meals { get; set; } = new();

        public bool IsLastDay => DayIndex == 6;
    }

    public class DashboardMealVm
    {
        public Guid DailyMealId { get; set; }

        public string MealType { get; set; }
        public string MealName { get; set; }

        public bool IsEaten { get; set; }
        public bool IsSubstituted { get; set; }
    }

    public class ToggleMealRequest
    {
        public Guid DailyMealId { get; set; }
        public bool IsEaten { get; set; }
    }


}
