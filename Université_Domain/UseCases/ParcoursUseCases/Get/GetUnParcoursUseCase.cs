using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.ParcoursUseCases.Get;

public class GetUnParcoursUseCase(IRepositoryFactory repository)
{
    
        public async Task<Parcours> ExecuteAsync(long parcoursId)
        {
            // Ensure the input is a valid student number (non-zero)
            if (parcoursId == 0)
            {
                throw new ArgumentException("Numéro d'étudiant is required", nameof(parcoursId));
            }

            // Search for the Etudiant by their NumEtud
            var parcours = await repository.ParcoursRepository().FindAsync(parcoursId);
            Console.WriteLine(parcours);

            // If no Etudiant is found, throw an exception
            
        
        

            return parcours; // Return true to indicate successful deletion
        }


        // Authorization check to verify if the user has the right role to perform the delete operation
        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }
}
