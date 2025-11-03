using System.ComponentModel.DataAnnotations;

namespace NotesBackend.DTOs
{
    /// <summary>
    /// Request DTO for creating a new note.
    /// </summary>
    public class CreateNoteRequest
    {
        /// <summary>
        /// Title of the note (required, max 200 chars).
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content/body of the note.
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for updating an existing note.
    /// </summary>
    public class UpdateNoteRequest
    {
        /// <summary>
        /// Title of the note (required, max 200 chars).
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content/body of the note.
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response DTO for returning note data to clients.
    /// </summary>
    public class NoteResponse
    {
        /// <summary>
        /// Unique identifier for the note.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the note.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content/body of the note.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last updated timestamp (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
