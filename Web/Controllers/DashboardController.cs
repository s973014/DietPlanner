using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{

    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserRepository _userRepository;

        public DashboardController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            string? userName = "Пользователь";
            string role = "User";

            var jwt = Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(jwt))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                var roleClaim = token.Claims.FirstOrDefault(c => c.Type == "role");

                if (userIdClaim != null)
                {
                    var userId = Guid.Parse(userIdClaim.Value);
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                        userName = user.Name;
                }

                if (roleClaim != null)
                    role = roleClaim.Value;
            }

            ViewData["UserName"] = userName;
            ViewData["UserRole"] = role;

            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }
    }

}
