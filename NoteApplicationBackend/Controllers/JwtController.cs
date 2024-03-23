using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApplicationBackend.Services;

namespace NoteApplicationBackend.Controllers
{
    [ApiController]
    
    public class JwtController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public JwtController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }


    [HttpPost("validate-token")]
    public IActionResult ValidateToken([FromBody] string token)
    {

        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token cannot be empty.");
        }

        bool isTokenValid = _jwtService.IsValid(token);

        if (isTokenValid)
        {
            return Redirect("/Token-Valid");
            //return Ok("Token is valid.");
        }
        else
        {
            return Redirect("/Token-Invalid");

            //return Unauthorized("Token is invalid or expired.");
            
        }
    }
    [HttpGet("Token-Valid")]
    public IActionResult Valıd()
    {
        return Ok(true);
    }
    [HttpGet("Token-Invalid")]
    public IActionResult Invalid()
    {
        return Ok(false);
    }

}

}