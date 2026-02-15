using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.ParcoursUseCases.Get;

public class GetAllParcoursUseCase(IRepositoryFactory repository)
{
    
    public async Task<List<Parcours>> ExecuteAsync()
    {
        // Ensure the input is a valid student number (non-zero)
        

        // Search for the Etudiant by their NumEtud
        var parcours = await repository.ParcoursRepository().FindAllAsync();

        // If no Etudiant is found, throw an exception
            
        
        

        return parcours; // Return true to indicate successful deletion
    }


    // Authorization check to verify if the user has the right role to perform the delete operation
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}
