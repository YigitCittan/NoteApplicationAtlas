namespace NoteApplicationBackend.Models
{
    public class UserPhoto
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required byte[] PhotoData { get; set; }
}
}