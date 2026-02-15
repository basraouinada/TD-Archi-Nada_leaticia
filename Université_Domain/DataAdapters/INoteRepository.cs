using Université_Domain.Entities;

namespace Université_Domain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
   // Task<Note> DeleteNote(List<Note> notes);
   Task SaveOrUpdateAsync(Note note);
    
}