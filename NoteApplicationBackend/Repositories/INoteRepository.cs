using NoteApplicationBackend.Models;

namespace NoteApplicationBackend.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<IEnumerable<Note>> GetUserNotesAsync(string userId);
        Task<Note> GetNoteByIdAsync(Guid id);
        Task<Note> AddNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(Guid id, Note updatedNote);
        Task<bool> DeleteNoteAsync(Guid id);
    }
}
