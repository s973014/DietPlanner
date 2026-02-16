namespace Web.Controllers
{
    using Application.Interfaces;
    using Domain.Entitites;
    using Domain.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    namespace Web.Controllers
    {
        [Authorize]
        public class ProfileController : Controller
        {
            private readonly IUserRepository _userRepository;
            private readonly IAllergyRepository _allergyRepository;
            private readonly IUnitOfWork _unitOfWork;

            public ProfileController(
                IUserRepository userRepository,
                IAllergyRepository allergyRepository,
                IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _allergyRepository = allergyRepository;
                _unitOfWork = unitOfWork;
            }

            [HttpGet]
            public async Task<IActionResult> Edit()
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound();

                var allergies = await _allergyRepository.GetAllAsync();

                var vm = new EditProfileVm
                {
                    Weight = user.Weight,
                    Height = user.Height,
                    ActivityLevel = user.ActivityLevel,
                    Goal = user.Goal,
                    Allergies = allergies.Select(a => new AllergyVm
                    {
                        Id = a.Id,
                        Name = a.Name,
                        IsSelected = user.Allergies.Any(ua => ua.Id == a.Id)
                    }).ToList()
                };

                return View(vm);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(EditProfileVm vm)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound();

                if (vm.Weight.HasValue &&
                    vm.Height.HasValue &&
                    vm.ActivityLevel.HasValue &&
                    vm.Goal.HasValue)
                {
                    user.UpdateProfile(
                        vm.Weight.Value,
                        vm.Height.Value,
                        vm.ActivityLevel.Value,
                        vm.Goal.Value
                    );
                }

                user.ClearAllergies();

                foreach (var allergyId in vm.SelectedAllergyIds)
                {
                    var allergy = await _allergyRepository.GetByIdAsync(allergyId);
                    if (allergy != null)
                        user.AddAllergy(allergy);
                }
                _userRepository.Update(user);

                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index", "Dashboard");
            }

 
            public IActionResult ChangePassword()
            {
                return View(new ChangePasswordVm());
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ChangePassword(ChangePasswordVm vm)
            {
                if (!ModelState.IsValid)
                    return View(vm);

                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound();

                if (!VerifyPassword(vm.OldPassword, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Неверный текущий пароль");
                    return View(vm);
                }

                user.SetPassword(HashPassword(vm.NewPassword));
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Пароль успешно изменён";
                return RedirectToAction("Edit");
            }

            private string HashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            private bool VerifyPassword(string password, string hash)
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }

        }


    }

    public class ChangePasswordVm
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
    public class EditProfileVm
    {
        public float? Weight { get; set; }
        public float? Height { get; set; }

        public ActivityLevel? ActivityLevel { get; set; }
        public GoalType? Goal { get; set; }

        public List<Guid> SelectedAllergyIds { get; set; } = new();

        public List<AllergyVm> Allergies { get; set; } = new();
    }

    public class AllergyVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }

}
