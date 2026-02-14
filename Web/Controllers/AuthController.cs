using Application.Interfaces;
using Domain.Entitites;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                TempData["Error"] = "Пароли не совпадают!";
                return RedirectToAction("Register");
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                TempData["Error"] = "Пользователь с таким email уже существует!";
                return RedirectToAction("Register");
            }

            if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            {
                TempData["Error"] = "Некорректная роль!";
                return RedirectToAction("Register");
            }
            var user = new User(
                request.Name,
                request.Email,
                HashPassword(request.Password),
                role
            );

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Регистрация успешна! Войдите в систему.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                TempData["Error"] = "Неверный email или пароль!";
                return RedirectToAction("Login");
            }

            var token = GenerateJwtToken(user);

            
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(8),
                SameSite = SameSiteMode.Strict,
                Secure = false
            });

            return RedirectToAction("Index", "Dashboard");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

    public class RegisterRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Role { get; set; } = "User";
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

