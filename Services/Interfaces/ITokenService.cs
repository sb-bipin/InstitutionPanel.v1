using InstitutionPanel.Models.Entities;

namespace InstitutionPanel.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
