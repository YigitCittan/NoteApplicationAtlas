using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApplicationBackend.Data;
using NoteApplicationBackend.Repositories;

namespace NoteApplicationBackend.Controllers
{
    
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RoleController> _logger;


    public RoleController(ILogger<RoleController> logger,
                          IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

        [HttpGet("api/Admin/GetRoles")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost("api/Admin/CreateRole")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExist = await _roleRepository.RoleExistsAsync(name);

            if(!roleExist)
            {
                var result = await _roleRepository.CreateRoleAsync(name);


                if (result)
                {
                    _logger.LogInformation($"The Role {name} has been added ");
                    return Ok(new { result = $"The Role {name} has been added " });
                }
                else
                {
                    _logger.LogInformation($"The Role {name} has not been added ");
                    return BadRequest(new { result = $"The Role {name} has not been added " });
                }

            }
            return BadRequest(new{error = "Role already exist"});
        }

        [HttpPost("api/Admin/AddUserToRole")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var result = await _roleRepository.AddUserToRoleAsync(userId, roleName);

            if (result)
            {
                return Ok("User has been added to the role");
            }
            else
            {
                _logger.LogInformation($"The Role {roleName} cannot be added to the user {userId}");
                return BadRequest("Role cannot be added");
            }
        }

        [HttpGet("api/Admin/GetUserRoles")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult>GetUserRoles(string email)
        {
            var roles = await _roleRepository.GetUserRolesAsync(email);
            if (roles != null)
            {
                return Ok(roles);
            }
            else
            {
                return BadRequest("User not found or has no roles");
            }
        }

        [HttpGet("api/Admin/GetUserRolesById")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult>GetUserRolesById(string userId)
        {
             var roles = await _roleRepository.GetUserRolesByIdAsync(userId);
            if (roles != null)
            {
                return Ok(roles);
            }
            else
            {
                return BadRequest("User not found or has no roles");
            }
        }

        [HttpPost("api/Admin/DeleteUserFromRole")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult> DeleteUserFromRole(string email, string roleName)
        {
            var result = await _roleRepository.RemoveUserFromRoleAsync(email, roleName);
            if (result)
            {
                return Ok("User has been removed from role");
            }
            else
            {
                return BadRequest("User not found or not in the specified role");
            }
        }
  
    }
}