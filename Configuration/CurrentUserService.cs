using InstitutionPanel.Core.Interfaces;
using System.Security.Claims;

namespace InstitutionPanel.Configuration
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId =>
            _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        public string UserType =>
            _httpContextAccessor.HttpContext?.User?
            .FindFirst("UserType")?.Value ?? "";

        public string InstitutionId =>
            _httpContextAccessor.HttpContext?.User?
            .FindFirst("InstitutionId")?.Value ?? "";

        public string UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        public string Email =>
            _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.Email)?.Value ?? "";

        public bool IsAuthenticated =>
                        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;


    }

}
