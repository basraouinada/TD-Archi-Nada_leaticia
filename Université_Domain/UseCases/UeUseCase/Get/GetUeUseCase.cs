using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.UeUseCase.Get;

public class GetUnUeUseCase(IRepositoryFactory repository)
{
    
    public async Task<Ue> ExecuteAsync(long ueId)
    {
        // Vérifie que l'identifiant de l'UE est valide (non nul)
        if (ueId == 0)
        {
            throw new ArgumentException("L'identifiant de l'UE est requis.", nameof(ueId));
        }

        // Recherche de l'UE par son ID
        var ue = await repository.UeRepository().FindAsync(ueId).ConfigureAwait(false);
    
        // Vérifie si l'UE existe
        if (ue == null)
        {
            throw new Exception("UE non trouvée.");
        }

        Console.WriteLine(ue);
        return ue; // Retourne l'UE trouvée
    }



    // Authorization check to verify if the user has the right role to perform the delete operation
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}