using Microsoft.AspNetCore.Identity;

namespace NoteApplicationBackend.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt {get; set;}
       

        public string? UserId { get; set; }        
        public IdentityUser? User { get; set; }
}
   
}