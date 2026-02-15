using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.NoteUseCase.Get;

public class GetEtudiantNotes(IRepositoryFactory noterepository)
{
    public async Task<List<Note>> ExecuteAsync(long numEtud,long ueId)
    {
        // Ensure the input is a valid student number (non-zero)
        if (numEtud == 0)
        {
            throw new ArgumentException("Numéro d'étudiant is required", nameof(numEtud));
        }

        // Search for the Etudiant by their NumEtud
        var notes = await noterepository.NoteRepository().FindByConditionAsync(e => e.EtudiantId.Equals(numEtud)&&e.UeId.Equals(ueId)/*&&e.Valeur.Equals(valeur)*/);

        // If no Etudiant is found, throw an exception
        if (!notes.Any())
        {
            Console.WriteLine("aucune note trouver pour ce étudiant dans cette ue ");
            return null;

        }
        
        

        return notes; // Return true to indicate successful deletion
    }


    // Authorization check to verify if the user has the right role to perform the delete operation
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}