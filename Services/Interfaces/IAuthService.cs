using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs.Auth;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ViewModels;

namespace InstitutionPanel.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string? Token)> RegisterAsync(RegisterDto model);
        Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto model);
        Task LogoutAsync(HttpContext httpContext);
        Task<(bool Success, string Message)> ChangePasswordAsync(string userId,
                                                                string currentPassword,
                                                                string newPassword);
        Task<ApiResponse<List<ApplicationUserVM>>> GetAllUsersAsync();
        Task<ApplicationUserVM> GetUserDetailsByIdAsync(Guid id);
        Task<ApiResponse<bool>> UpdateUserAsync(UserDTO model);

    }
}
