using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.UeUseCase.Update;

public class UpdateUeUseCase(IRepositoryFactory repository)
{
    public async Task<Ue> ExecuteAsync(long parcoursId,Ue ue){
        // Ensure the input is a valid student number (non-zero)
        if (parcoursId == 0)
        {
            throw new ArgumentException("Numéro d'étudiant is required", nameof(parcoursId));
        }

        // Search for the Etudiant by their NumEtud
        var pr = await repository.UeRepository().FindAsync(parcoursId);

       
        
        // If no Etudiant is found, throw an exception
        if (pr==null)
        {
            Console.WriteLine("aucune Ue trouver pour ce avec de id   ");
            return null;

        }
        pr.NumeroUe = ue.NumeroUe;
        pr.Intitule = ue.Intitule;
        Console.WriteLine(pr);
        await repository.UeRepository().UpdateAsync(pr);
        
        

        return pr; // Return true to indicate successful deletion
    }


    // Authorization check to verify if the user has the right role to perform the delete operation
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}