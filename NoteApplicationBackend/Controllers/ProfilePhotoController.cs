using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApplicationBackend.Repositories;
using NoteApplicationBackend.Services;

namespace NoteApplicationBackend.Controllers
{
[ApiController]
[Authorize]
public class ProfilePhotoController : ControllerBase
{
    private readonly PhotoService _photoService;
    private readonly IUserRepository _userRepository;

    public ProfilePhotoController(PhotoService photoService,
                                  IUserRepository userRepository)
    {
        _photoService = photoService;
        _userRepository = userRepository;
    }
    
    [HttpGet("api/GetProfilePhoto")]
    public async Task<IActionResult> GetProfilePhoto()
    {
        var claims = HttpContext.User.Claims.ToList();
        var userId = claims.First().Value;
        var user = await _userRepository.GetUserById(userId);
        if(user ==null)
        {
            return BadRequest("User not exist");
        }
        string photoBase64 =await _photoService.GetUserPhoto(userId);
        return Ok(photoBase64);
    }

    [HttpPost("api/AddProfilePhoto")]
    public async Task<IActionResult> AddProfilePhoto(string base64)
    {
        var claims = HttpContext.User.Claims.ToList();
        var userId = claims.First().Value;
        var user = await _userRepository.GetUserById(userId);
        if(user ==null)
        {
            return BadRequest("User not exist");
        }
        await _photoService.AddProfilePhoto(userId,base64);
        return Ok();
        
    }

    [HttpPut("api/UpdateProfilePhoto")]
    public async Task<IActionResult> UpdateProfilePhoto(string base64)
    {
        var claims = HttpContext.User.Claims.ToList();
        var userId = claims.First().Value;
        var user = await _userRepository.GetUserById(userId);
        if(user == null)
        {
            return BadRequest("User not exist");
        }

        try
        {
            await _photoService.UpdateProfilePhoto(userId, base64);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("api/DeleteProfilePhoto")]
    public async Task<IActionResult> DeleteProfilePhoto()
    {
        var claims = HttpContext.User.Claims.ToList();
        var userId = claims.First().Value;
        var user = await _userRepository.GetUserById(userId);
        if(user == null)
        {
            return BadRequest("User not exist");
        }

        try
        {
            await _photoService.DeleteProfilePhoto(userId);
            return Ok("Profil fotoğrafı başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
}

  

}
}