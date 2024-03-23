using System.ComponentModel.DataAnnotations;
namespace NoteApplicationBackend.DTOs
{
    public class UserLoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password {get; set;}
    }
}