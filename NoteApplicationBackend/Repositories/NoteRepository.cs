using Microsoft.EntityFrameworkCore;
using NoteApplicationBackend.Data;
using NoteApplicationBackend.Models;

namespace NoteApplicationBackend.Repositories
{

    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetUserNotesAsync(string userId)
        {
            return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            return await _context.Notes.FindAsync(id) ?? throw new Exception("ID not found");
        }

        public async Task<Note> AddNoteAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> UpdateNoteAsync(Guid id, Note updatedNote)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                return false;

            note.Title = updatedNote.Title;
            note.Content = updatedNote.Content;
            note.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                return false;

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }
        
        private bool NoteExists(Guid id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }

    }
}
