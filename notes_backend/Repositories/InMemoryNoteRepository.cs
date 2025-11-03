using System.Collections.Concurrent;
using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    /// <summary>
    /// Thread-safe in-memory repository for notes. Suitable for development and testing.
    /// </summary>
    public class InMemoryNoteRepository : INoteRepository
    {
        private readonly ConcurrentDictionary<Guid, Note> _store = new();

        public Note Create(Note note)
        {
            _store[note.Id] = note;
            return note;
        }

        public bool Delete(Guid id)
        {
            return _store.TryRemove(id, out _);
        }

        public IEnumerable<Note> GetAll()
        {
            // Order by CreatedAt descending for convenience
            return _store.Values.OrderByDescending(n => n.CreatedAt);
        }

        public Note? GetById(Guid id)
        {
            _store.TryGetValue(id, out var note);
            return note;
        }

        public Note? Update(Note note)
        {
            if (!_store.ContainsKey(note.Id))
            {
                return null;
            }
            _store[note.Id] = note;
            return note;
        }
    }
}
