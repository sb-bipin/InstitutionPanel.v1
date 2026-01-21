using InstitutionPanel.Core.Interfaces;
using InstitutionPanel.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace InstitutionPanel.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //public async Task GetByIdAsync(string userId)
        //{
        //    return await _userManager.FindByIdAsync(userId);
        //}

        //public async Task<bool> GetByEmailAsync(string email)
        //{
        //    return await _userManager.FindByEmailAsync(email);
        //}

        public async Task<IEnumerable> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> CreateAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        //public async Task DeleteAsync(string userId)
        //{
        //    var user = await GetByIdAsync(userId);
        //    if (user == null) 
        //        return false;

        //    var result = await _userManager.DeleteAsync(user);
        //    return result.Succeeded;
        //}

        //public async Task ExistsAsync(string email)
        //{
        //    var user = await GetByEmailAsync(email);
        //    return user != null;
        //}
    }
}
