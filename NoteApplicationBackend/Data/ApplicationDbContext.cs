using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteApplicationBackend.Models;

namespace NoteApplicationBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<UserPhoto> userPhoto { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
    
    }
}
