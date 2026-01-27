using InstitutionPanel.Models.Entities;
using System.Collections;

namespace InstitutionPanel.Core.Interfaces
{
    public interface IUserRepository
    {
        public interface IUserRepository
        {
            Task<IEnumerable> GetAllAsync();
            Task<bool> CreateAsync(ApplicationUser user, string password);
            Task<bool> UpdateAsync(ApplicationUser user);

        }
    }
}
