using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NoteApplicationBackend.Configurations;
using NoteApplicationBackend.DTOs;
using NoteApplicationBackend.Repositories;
using NoteApplicationBackend.Services;

namespace NoteApplicationBackend.Controllers
{
   
[ApiController]

public class AuthManagementController : ControllerBase
{
    
    private readonly JwtService _jwtService;
    private readonly IUserRepository _userRepository;
    public AuthManagementController(JwtService jwtService,
                                    IUserRepository userRepository)
                                    
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
    }
    
    [HttpPost("api/AuthManagement/Register")]   
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
    {
        //Request accuracy check
        if(ModelState.IsValid)
        {
            //database email query
            var emailExist = await _userRepository.QueryUserByEmailAsync(requestDto.Email);
            if(emailExist != null)
                {
                    return BadRequest("Email already exist");
                }
            //create new IdentityUser
            var newUser = new IdentityUser()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };

            var isCreated = await _userRepository.CreateUserAsync(newUser, requestDto.Password);


            if(isCreated.Succeeded)
            {   
                // user default role assign
                var resultRoleAddition = await _userRepository.AddToRoleAsync(newUser,"User");
                if(resultRoleAddition.Succeeded)
                {

                    //generate new JWT for new user
                    var authResult = await _jwtService.GenerateJwtToken(newUser);
                    string token = authResult.Token;

                    return Ok(new RegistrationRequestResponse
                    {
                        Result = true,
                        Token = token
                    });
                }
                else
                BadRequest("Default role (user) not exist.");
                
            }
                
                
            return BadRequest("Error creating the user");   
        }
        return BadRequest("Invalid request");
    }
    
    [HttpPost("api/AuthManagement/Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
    {
        //Request accuracy check
        if(ModelState.IsValid)
        {
            //email query
            var existingUser = await _userRepository.QueryUserByEmailAsync(requestDto.Email);
            if(existingUser == null)
            {
                return BadRequest("Invalid auth");
            }

            //check user password
            var isPasswordValid = await _userRepository.CheckPasswordAsync(existingUser, requestDto.Password);
            if(isPasswordValid)
            {   

                //generate token for user
                var authResult = await _jwtService.GenerateJwtToken(existingUser);
                string token = authResult.Token;
                if (await _userRepository.IsUserLockedOutAsync(existingUser.Id))
                {
                    return BadRequest("Account is Deleted");
                }
                return Ok(new RegistrationRequestResponse
                {
                    Result = true,
                    Token = token
                });
            }

        }
        return BadRequest("Invalid password");
    }
}

}