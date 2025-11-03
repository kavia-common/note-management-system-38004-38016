using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Models
{
    /// <summary>
    /// Domain model representing a Note entity.
    /// </summary>
    public class Note
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
        /// Note content/body.
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
