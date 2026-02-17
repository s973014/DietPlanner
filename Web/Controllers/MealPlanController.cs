using Application.Interfaces;
using Application.Services;
using Domain.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class MealPlanController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMealPlanService _mealPlanService;

        public MealPlanController(
            IUserRepository userRepository,
            IWeeklyPlanRepository weeklyPlanRepository,
            IUnitOfWork unitOfWork,
            IMealPlanService mealPlanService)
        {
            _userRepository = userRepository;
            _weeklyPlanRepository = weeklyPlanRepository;
            _unitOfWork = unitOfWork;
            _mealPlanService = mealPlanService;
        }

        public async Task<IActionResult> Select()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null) return Unauthorized();

            bool canSelectPlan =
                user.Weight.HasValue &&
                user.Height.HasValue &&
                user.ActivityLevel.HasValue &&
                user.Goal.HasValue;

            var plansVm = canSelectPlan
                ? await _mealPlanService.BuildSelectablePlansAsync(user)
                : new List<WeeklyPlanPreviewVm>();

            return View(new MealPlanSelectVm
            {
                CanSelectPlan = canSelectPlan,
                WeeklyPlans = plansVm
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(Guid planId)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var user = await _userRepository.GetByIdAsync(userId);
            var plan = await _weeklyPlanRepository.GetByIdAsync(planId);

            if (user == null || plan == null) return NotFound();


            await _mealPlanService.AssignPlanToUserAsync(user.Id, plan.Id);

            await _unitOfWork.SaveChangesAsync();

            return Redirect("/Dashboard");
        }
    }

    public class MealPlanSelectVm
    {
        public bool CanSelectPlan { get; set; }
        public List<WeeklyPlanPreviewVm> WeeklyPlans { get; set; } = new();
    }
}
