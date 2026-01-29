using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs.Auth;
using InstitutionPanel.Models.DTOs.Institution;
using InstitutionPanel.Models.ViewModels;
using InstitutionPanel.Services.Implementations;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionPanel.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Register User Section
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
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
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            var userType = HttpContext.Session.GetString("UserType");
            if (!string.IsNullOrEmpty(userType))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
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

            HttpContext.Response.Cookies.Append("JWToken", result.Token!,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, 
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout section
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);
            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToAction("Login");
        }
        #endregion

        #region User Management Section

        public async Task<IActionResult> UserView(Guid id)
        {
            var user = await _authService.GetUserDetailsByIdAsync(id);

            if (user == null)
                return NotFound();

            return PartialView("_UserView", user);
        }

        public async Task<IActionResult> UserList()
        {
            var response = await _authService.GetAllUsersAsync();

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new List<InstitutionVM>());
            }

            return View(response.Data);
        }

        public async Task<IActionResult> EditUser(Guid id)
        {
            var user = await _authService.GetUserDetailsByIdAsync(id);

            if (user == null)
                return NotFound();

            return PartialView("_EditUser", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserDTO model)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditUser", model);

            var result = await _authService.UpdateUserAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", "Failed to update user.");
                return PartialView("_EditUser", model);
            }

            TempData["SuccessMessage"] = "User updated successfully.";
            return Json(new { success = true });
        }


        #endregion
    }
}