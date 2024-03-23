using Microsoft.AspNetCore.Identity;

namespace NoteApplicationBackend.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string email);
        Task<IEnumerable<string>> GetUserRolesByIdAsync(string userId);
        Task<bool> RemoveUserFromRoleAsync(string email, string roleName);
        IEnumerable<IdentityRole> GetAllRoles();
    }
}
