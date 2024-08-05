using ECommerce.Entities;
using ECommerce.Repositories.Abstract;
using ECommerce.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Concrete
{
    public class UserRepository : IUserReposiory<ApplicationUser>
    {
        private readonly ECommerceDbContext _context;

        public UserRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await _context.ApplicationUsers.ToListAsync();
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
                return await _context.ApplicationUsers.FindAsync(userId);
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
                _context.ApplicationUsers.Add(user);
                await _context.SaveChangesAsync();
                return user;
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
                _context.ApplicationUsers.Update(user);
                await _context.SaveChangesAsync();
                return user;
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
                var user = await _context.ApplicationUsers.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                _context.ApplicationUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user by ID: {userId}", ex);
            }
        }
    }
}
