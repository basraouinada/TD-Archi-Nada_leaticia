using Université_Domain.DataAdapters.DataAdaptersFactory;

namespace Université_Domain.UseCases.UeUseCase.Create;

using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using Université_Domain.Exceptions.UeExceptions;


public class CreateUeUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var ue = new Ue { NumeroUe = numeroUe, Intitule = intitule };
        return await ExecuteAsync(ue);
    }
    
    public async Task<Ue> ExecuteAsync(Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue createdUe = await repositoryFactory.UeRepository().CreateAsync(ue);
        await repositoryFactory.UeRepository().SaveChangesAsync(); // Sauvegarde asynchrone
        return createdUe;
    }
    
    private async Task CheckBusinessRules(Ue ue)
    {
        ArgumentNullException.ThrowIfNull(ue, nameof(ue));
        ArgumentNullException.ThrowIfNull(ue.NumeroUe, nameof(ue.NumeroUe));
        ArgumentNullException.ThrowIfNull(ue.Intitule, nameof(ue.Intitule));
        
        // Vérification si une UE avec le même numéro existe
        var existingUes = await repositoryFactory.UeRepository().FindByConditionAsync(u => u.NumeroUe == ue.NumeroUe);
        if (existingUes.Any()) 
            throw new DuplicateNumeroUeException($"Le numéro {ue.NumeroUe} est déjà utilisé par une autre UE.");
        
        // Validation de l'intitulé (> 3 caractères)
        if (ue.Intitule.Length <= 3) 
            throw new InvalidIntituleUeException($"L'intitulé '{ue.Intitule}' est trop court. Il doit contenir plus de 3 caractères.");
    }
    
    // adding for security
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}