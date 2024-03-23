namespace NoteApplicationBackend.DTOs
{
    public class UserProfileChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

}