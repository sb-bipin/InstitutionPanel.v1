using InstitutionPanel.Models.Entities;
using System.Collections;

namespace InstitutionPanel.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task GetByIdAsync(string userId);
        Task GetByEmailAsync(string email);
        Task<IEnumerable> GetAllAsync();
        Task CreateAsync(ApplicationUser user, string password);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string userId);
        Task ExistsAsync(string email);
    }
}
