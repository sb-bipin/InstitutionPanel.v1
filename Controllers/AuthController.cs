using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionPanel.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #region Register User Section

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register() => View();


        [AllowAnonymous]
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful! Please login.";

            return RedirectToAction("Login");
        }


        #endregion

        #region Login User section


        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login() => View();


        [AllowAnonymous]
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            HttpContext.Session.SetString("JWToken", result.Token!);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Logout section

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            HttpContext.Session.Remove("JWToken");

            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToAction("Login");
        }

        #endregion
    }
}
