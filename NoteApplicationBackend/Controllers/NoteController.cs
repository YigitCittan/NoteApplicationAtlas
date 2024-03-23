using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApplicationBackend.Data;
using NoteApplicationBackend.Models;
using NoteApplicationBackend.DTOs;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NoteApplicationBackend.Repositories;

namespace NoteApplicationBackend.Controllers
{
    [ApiController]
    [Authorize] // Needs user login
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        public NoteController(INoteRepository noteRepository)
        {
             _noteRepository = noteRepository;
        }


        [HttpGet("api/GetAllNotes")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles= "Admin")]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes()
        {
            //get all notes
            var allNotes = await _noteRepository.GetAllNotesAsync();

            return Ok(allNotes);

      
        }
      
        [HttpGet("api/GetNotes")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {   
            //get user claims and get userId from user claims
            var userId = HttpContext.User.Claims.First().Value;
            var userNotes = await _noteRepository.GetUserNotesAsync(userId);
            return Ok(userNotes);
        }

        [HttpGet("api/GetNoteById")]
        public async Task<ActionResult<Note>> GetNote(Guid id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            var userId = HttpContext.User.Claims.First().Value;
            if (note.UserId != userId)
            {
                return BadRequest(); // Authrozition failure
            }

            return Ok(note);
        }

        
        [HttpPost("api/PostNote")]
        public async Task<ActionResult<Note>> PostNote(NoteDto noteDto )
        {
            //get user claims
            var userId = HttpContext.User.Claims.First().Value;
            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
            };
            var addedNote = await _noteRepository.AddNoteAsync(note);
            return CreatedAtAction(nameof(GetNote), new { id = addedNote.Id }, addedNote);
        }


        [HttpPut("api/UpdateNote{id}")]
        public async Task<IActionResult> PutNote(Guid id, [FromBody] NoteDto noteDto)
        { 
            
            var updatedNote = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                UpdatedAt = DateTime.UtcNow,
            };
            var success = await _noteRepository.UpdateNoteAsync(id, updatedNote);
            if (!success)
            {
                return NotFound();
            }
            return Ok(updatedNote);
        }

     

        [HttpDelete("api/DeleteNote{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var success = await _noteRepository.DeleteNoteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        
    }
}
