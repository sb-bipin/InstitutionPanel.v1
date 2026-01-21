using InstitutionPanel.Models.DTOs;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InstitutionPanel.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string Message, string? Token)> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return (false, "User already exists", null);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Self"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return (false, "Registration failed", null);
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            return (true, "Registration successful", token);
        }

        public async Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return (false, "Invalid credentials", null);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return (false, "Invalid credentials", null);
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            return (true, "Login successful", token);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);

            return result.Succeeded
                ? (true, "Password changed successfully")
                : (false, "Failed to change password");
        }
    }
}
