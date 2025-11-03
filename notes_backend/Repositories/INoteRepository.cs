using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    /// <summary>
    /// Abstraction for a Note repository to allow swapping persistence implementations.
    /// </summary>
    public interface INoteRepository
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Create a new note.
        /// </summary>
        /// <param name="note">The note to create.</param>
        /// <returns>The created note.</returns>
        Note Create(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Get a note by its Id.
        /// </summary>
        /// <param name="id">Note Id.</param>
        /// <returns>The note if found; otherwise null.</returns>
        Note? GetById(Guid id);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Get all notes.
        /// </summary>
        /// <returns>Enumerable of notes.</returns>
        IEnumerable<Note> GetAll();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Update an existing note.
        /// </summary>
        /// <param name="note">The note with updated fields.</param>
        /// <returns>The updated note, or null if the note does not exist.</returns>
        Note? Update(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Delete a note by Id.
        /// </summary>
        /// <param name="id">Note Id.</param>
        /// <returns>True if deleted; false if not found.</returns>
        bool Delete(Guid id);
    }
}
