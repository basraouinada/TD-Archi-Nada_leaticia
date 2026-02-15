using Université_Domain.DataAdapters;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;

namespace Université_Domain.UseCases.EtudiantUseCases.Get;

public class GetTousLesEtudiantsUseCase(IRepositoryFactory factory)
{
    public async Task<List<Etudiant?>> ExecuteAsync()
    {
        await CheckBusinessRules();
        List<Etudiant> etudiant = await factory.EtudiantRepository().FindAllAsync();
        if (etudiant.Count == 0)
        { 
            throw new InvalidOperationException("Failed to retrieve students; result is null.");
            
        }
        return etudiant;
    }
    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IEtudiantRepository etudiantRepository=factory.EtudiantRepository();
        ArgumentNullException.ThrowIfNull(etudiantRepository);
    }
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}