using InstitutionPanel.Models.Entities;
using System.Collections;

namespace InstitutionPanel.Core.Interfaces
{
    public interface IUserRepository
    {
        public interface IUserRepository
        {
            //Task GetByIdAsync(string userId);
            //Task GetByEmailAsync(string email);
            Task<IEnumerable> GetAllAsync();
            Task<bool> CreateAsync(ApplicationUser user, string password);
            Task<bool> UpdateAsync(ApplicationUser user);
            //Task DeleteAsync(string userId);
            //Task ExistsAsync(string email);
        }
    }
}
