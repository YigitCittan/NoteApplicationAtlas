using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteApplicationBackend.Configurations;
using NoteApplicationBackend.DTOs;
using NoteApplicationBackend.Repositories;
using NoteApplicationBackend.Services;

namespace NoteApplicationBackend.Controllers
{
    [ApiController]
    [Authorize] 

    public class UserProfileController : ControllerBase
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly IUserRepository _userRepository;

        public UserProfileController(IOptionsMonitor<JwtConfiguration> optionsMonitor,
                                    IUserRepository userRepository
                                    )                         
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _userRepository = userRepository;
        }
        [HttpGet("api/Admin/AllUsers")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]

        public async Task<IActionResult> GetAllUsers()
        {
            var users =  await _userRepository.GetAllUsers();
            return Ok(users);   
        }
        
        [HttpGet("api/AuthManagement/GetById")]
        public async Task<IActionResult> GetProfileById(string Id)
        {
            
            if (Id == null)
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            var user = await _userRepository.GetUserById(Id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        [HttpGet("api/AuthManagement/GetProfile")]
        public async Task<IActionResult> GetOwnProfileId()
        {
            
            //get user claims and get userId from user claims
            var claims = HttpContext.User.Claims.ToList();
            var userId = claims.First().Value;
        

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not exist");
            }
            return Ok(user);
            
        }

        [HttpPatch("api/AuthManagement/ChangeUserName")]
        public async Task<IActionResult> UpdateOwnProfileUserName([FromBody] UserProfileUpdateDto profileDto)
        {
            if (profileDto == null)
            {
                return BadRequest("Güncellenecek profil bilgileri geçerli değil");
            }

            //get user claims and get userId from user claims
            var claims = HttpContext.User.Claims.ToList();
            var userId = claims.First().Value;


            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            try
            {
                //update username
                var updatedUser = await _userRepository.UpdateUserProfileUserName(userId, profileDto);
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Güncelleme sırasında hata oluştu: {ex.Message}");
                return StatusCode(500, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpPatch("api/AuthManagement/ChangeEmail")]
        public async Task<IActionResult> UpdateOwnProfileEmail([FromBody] UserProfileUpdateDto profileDto)
        {
            if (profileDto == null)
            {
                return BadRequest("Güncellenecek profil bilgileri geçerli değil");
            }

            //get user claims and get userId from user claims
            var claims = HttpContext.User.Claims.ToList();
            var userId = claims.First().Value;


            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            try
            {
                //update username
                var updatedUser = await _userRepository.UpdateUserProfileEmail(userId, profileDto);
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Güncelleme sırasında hata oluştu: {ex.Message}");
                return StatusCode(500, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpPatch("api/AuthManagement/ChangePassword")]
        public async Task<IActionResult> UpdateOwnProfilePassword([FromBody] UserProfileChangePasswordDto profileDto)
        {
            if (profileDto == null)
            {
                return BadRequest("Güncellenecek profil bilgileri geçerli değil");
            }

            //get user claims and get userId from user claims
            var claims = HttpContext.User.Claims.ToList();
            var userId = claims.First().Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            try
            {
                var updatedUser = await _userRepository.UpdateUserPassword(userId, profileDto );
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Güncelleme sırasında hata oluştu: {ex.Message}");
                return StatusCode(500, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
        

        [HttpDelete("api/AuthManagement/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims.First().Value;
            var user = await _userRepository.GetUserById(userId);
            if(user ==null)
            {
                return BadRequest("User not exist");
            }
            var result = await _userRepository .DeleteUserAsync(user);
            if(result.Succeeded)
            {
                return Ok("User has been removed");
            }
            return BadRequest("User cannot be remove");
        }       

        [HttpDelete("api/AuthManagement/DeleteUserByMail")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult> DeleteUserByMail(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if(user ==null)
            {
                
                return BadRequest("User not exist");
            }
            var result = await _userRepository.DeleteUserAsync(user);
            if(result.Succeeded)
            {
                return Ok("User has been removed");
            }
            return BadRequest("User cannot be remove");
        }       
    
        [HttpPut("api/AuthManagement/Deactivate")]
        public async Task<IActionResult> DeactivateUser()
        {
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims.First().Value;
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTimeOffset.MaxValue; 
            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }


        [HttpPut("{id}api/AuthManagement/DeactivateById")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<IActionResult> DeactivateUserByMail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTimeOffset.MaxValue; 
            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
        
    
        [HttpPut("{id}api/AuthManagement/activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = null; 
            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
         [HttpGet("api/Islockedout/{userId}")]
        public async Task<bool> IsLockedOut(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return false; 
            }

            return user.LockoutEnabled && await _userRepository.IsLockedOutAsync(user);
        }
    }
}