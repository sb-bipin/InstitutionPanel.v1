using InstitutionPanel.Models.DTOs;

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
    }
}
