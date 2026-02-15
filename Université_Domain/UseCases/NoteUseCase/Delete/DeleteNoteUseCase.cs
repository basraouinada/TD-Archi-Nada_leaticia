using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;
using Université_Domain.Exceptions.EtudiantExceptions;

namespace Université_Domain.UseCases.NoteUseCase.Delete;

public class DeleteNoteUseCase (IRepositoryFactory noterepository)
{
    
    // Executes the delete operation for a given Etudiant (based on NumEtud)
    public async Task<bool> ExecuteAsync(long numEtud,long ueId, float valeur)
    {
        // Ensure the input is a valid student number (non-zero)
        if (numEtud == 0)
        {
            throw new ArgumentException("Numéro d'étudiant is required", nameof(numEtud));
        }

        // Search for the Etudiant by their NumEtud
        var notes = await noterepository.NoteRepository().FindByConditionAsync(e => e.EtudiantId.Equals(numEtud)&&e.UeId.Equals(ueId)&&e.Valeur.Equals(valeur));

        // If no Etudiant is found, throw an exception
        if (!notes.Any())
        {
            throw new EtudiantNotFoundException($"aucune note trouver pour ce étudiant dans cette ue ");
        }

        // If the Etudiant exists, delete them
        //var etudiantToDelete = etudiant.First(); // We expect only one result, as NumEtud should be unique
        //await noterepository.NoteRepository().DeleteNote(notes);

        // Commit the transaction (save changes to the database)
        await noterepository.SaveChangesAsync();

        return true; // Return true to indicate successful deletion
    }


    // Authorization check to verify if the user has the right role to perform the delete operation
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}

