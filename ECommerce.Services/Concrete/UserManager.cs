using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Concrete
{
    public class UserManager : IUserService<ApplicationUser>
    {
        private readonly IUserReposiory<ApplicationUser> _userRepository;

        public UserManager(IUserReposiory<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all users", ex);
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _userRepository.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user by ID: {userId}", ex);
            }
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                return await _userRepository.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user", ex);
            }
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                return await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                return await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user by ID: {userId}", ex);
            }
        }
    }
}
