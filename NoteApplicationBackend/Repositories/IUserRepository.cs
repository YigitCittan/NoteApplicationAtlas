using Microsoft.AspNetCore.Identity;
using NoteApplicationBackend.DTOs;

namespace NoteApplicationBackend.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityUser> QueryUserByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);
        Task<bool> IsLockedOutAsync(IdentityUser user);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password); 
        Task<IdentityUser?> GetUserById(string userId);
        Task<IdentityUser?> UpdateUserProfileUserName(string userId, UserProfileUpdateDto profileDto);
        Task<IdentityUser?> UpdateUserProfileEmail(string userId, UserProfileUpdateDto profileDto);
        Task<IdentityUser?> UpdateUserPassword(string userId, UserProfileChangePasswordDto profileDto);
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IdentityResult> DeleteUserAsync(IdentityUser user);
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);
        Task<bool> IsUserLockedOutAsync(string userId);
    }
}
