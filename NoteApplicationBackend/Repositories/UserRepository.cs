using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteApplicationBackend.DTOs;

namespace NoteApplicationBackend.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        public Task<bool> IsLockedOutAsync(IdentityUser user)
        {
            return _userManager.IsLockedOutAsync(user);
        }

        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityUser?> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<IdentityUser?> UpdateUserProfileUserName(string userId, UserProfileUpdateDto profileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.UserName = profileDto.UserName;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? user : null;
        }

        public async Task<IdentityUser?> UpdateUserProfileEmail(string userId, UserProfileUpdateDto profileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.Email = profileDto.Email;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? user : null;
        }

        public async Task<IdentityUser?> UpdateUserPassword(string userId, UserProfileChangePasswordDto profileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, profileDto.CurrentPassword, profileDto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return null;
            }
            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded ? user : null;
        }
        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
        public Task<IdentityResult> DeleteUserAsync(IdentityUser user)
        {
            return _userManager.DeleteAsync(user);
        }
        public Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return _userManager.UpdateAsync(user);
        }
        public async Task<bool> IsUserLockedOutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }

            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task<IdentityUser> QueryUserByEmailAsync(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
           return user;
           
        }
    }
}
