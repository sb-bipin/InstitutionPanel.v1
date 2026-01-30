using InstitutionPanel.Core.Interfaces;
using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs.Auth;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ValueObject;
using InstitutionPanel.Models.ViewModels;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InstitutionPanel.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUser;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ICurrentUserService currentUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _currentUser = currentUser;
        }

        public async Task<(bool Success, string Message, string? Token)> RegisterAsync(RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return (false, "User already exists", null);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = Email.From(model.Email),
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Self",
                InstitutionId = model.InstitutionId,
                UserType = _currentUser.UserType == "SuperAdmin" ? model.UserType : UserType.InstitutionAdmin
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, error, null);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var token = await _tokenService.GenerateTokenAsync(user);
            return (true, "Registration successful", token);
        }

        public async Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return (false, "Email or password cannot be empty", null);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return (false, "Invalid credentials", null);

            if (!user.IsActive)
                return (false, "User is inactive", null);

            var result = await _signInManager
                                        .PasswordSignInAsync(model.Email,
                                                            model.Password,
                                                            model.RememberMe,
                                                            lockoutOnFailure: false);

            if (!result.Succeeded)
                return (false, "Invalid credentials", null);

            var token = await _tokenService.GenerateTokenAsync(user);
            return (true, "Login successful", token);
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await _signInManager.SignOutAsync();

            httpContext.Session.Clear();

            if (httpContext.Request.Cookies.ContainsKey("JWToken"))
            {
                httpContext.Response.Cookies.Delete("JWToken");
            }

        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
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

        public async Task<ApiResponse<List<ApplicationUserVM>>> GetAllUsersAsync()
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(_currentUser.InstitutionId))
            {
                usersQuery = usersQuery.Where(u => u.InstitutionId.ToString() == _currentUser.InstitutionId);
            }

            var users = await usersQuery
                            .Select(user => new ApplicationUserVM
                            {
                                Id = Guid.Parse(user.Id),
                                UserName = user.UserName,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                InstitutionId = user.InstitutionId,
                                IsActive = user.IsActive,
                                UserType = user.UserType,
                                CreatedAt = user.CreatedAt.ToString(),
                                CreatedBy = user.CreatedBy,
                                UpdatedAt = user.UpdatedAt.HasValue ? user.UpdatedAt.Value.ToString() : null,
                                UpdatedBy = user.UpdatedBy,
                                InstitutionName = user.Institution.Name
                            })
                            .ToListAsync();

            return ApiResponse<List<ApplicationUserVM>>.SuccessResponse(users);

        }

        public async Task<ApplicationUserVM> GetUserDetailsByIdAsync(Guid id)
        {
            return await _userManager.Users.AsNoTracking()
                                .Where(u => u.Id == id.ToString())
                                .Select(user => new ApplicationUserVM
                                {
                                    Id = Guid.Parse(user.Id),
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    InstitutionId = user.InstitutionId,
                                    IsActive = user.IsActive,
                                    UserType = user.UserType,
                                    CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd"),
                                    CreatedBy = user.CreatedBy,
                                })
                                .SingleOrDefaultAsync();


        }
        public async Task<ApiResponse<bool>> UpdateUserAsync(UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user is null)
                return ApiResponse<bool>.FailureResponse("User not found");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsActive = model.IsActive;
            user.UserType = model.UserType;
            user.InstitutionId = model.InstitutionId;

            var result = await _userManager.UpdateAsync(user);

            return ApiResponse<bool>.SuccessResponse(true, "User updated successfully");
        }

    }
}
