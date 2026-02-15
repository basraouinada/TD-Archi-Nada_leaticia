using Université_Domain.DataAdapters;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;
using Université_Domain.Exceptions.EtudiantExceptions;
using Université_Domain.Exceptions.ParcoursExceptions;

namespace Université_Domain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory parcoursRepository)
{
    public async Task<Parcours> ExecuteAsync(int id_parcours, string nom, int année_parcours)
    {
        var parcours = new Parcours(){Id = id_parcours, NomParcours = nom, AnneeFormation = année_parcours};
        return await ExecuteAsync(parcours);
    }

    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        ArgumentNullException.ThrowIfNull(parcours.Id);
        
        // On recherche un étudiant avec le même numéro étudiant
        List<Parcours> existe = await parcoursRepository.ParcoursRepository().FindByConditionAsync(e=>e.NomParcours.Equals(parcours.NomParcours));
        List<Parcours> parcour = await parcoursRepository.ParcoursRepository().FindByConditionAsync(e=>e.AnneeFormation.Equals(parcours.AnneeFormation)) ;
        // Si un étudiant avec le même numéro étudiant existe déjà, on lève une exception personnalisée
        //if (existe is {Count:>=0} && parcour is {Count:>=0}) throw new DuplicateNomParcoursException(parcours.NomParcours+ " - ce  parcours deja existant");
        
        
        var duplicates = await parcoursRepository.ParcoursRepository().FindByConditionAsync(e => 
            e.NomParcours.Equals(parcours.NomParcours) && e.AnneeFormation.Equals(parcours.AnneeFormation));

        /*if (duplicates is{Count:0})
            throw new DuplicateNomParcoursException($"{parcours.NomParcours} - ce parcours est déjà existant");*/
        
        if (parcours.NomParcours.Length < 3) throw new InvalidNomEtudiantException(parcours.NomParcours +" incorrect - Le nom d'un parcours doit contenir plus de 3 caractères");
    }
   

    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours et = await parcoursRepository.ParcoursRepository().CreateAsync(parcours);
        parcoursRepository.SaveChangesAsync().Wait();
        return et;
    }
    // adding for security
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}