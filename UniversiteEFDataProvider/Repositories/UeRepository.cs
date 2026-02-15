using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{

    // Ajouter une note pour une UE
    public async Task AjouterNoteAsync(long ueId, long etudiantId, float valeur)
    {
        var ue = await FindAsync(ueId);
        if (ue != null)
        {
            var note = new Note
            {
                UeId = ueId,
                EtudiantId = etudiantId,
                Valeur = valeur
            };
            Context.Notes.Add(note);
            await SaveChangesAsync();
        }
    }

    public async Task AjouterNoteAsync(Ue ue, Etudiant etudiant, float valeur)
    {
        await AjouterNoteAsync(ue.Id, etudiant.Id, valeur);
    }


    // Associer une UE à un parcours
    public async Task AssocierUeAuParcoursAsync(long ueId, long parcoursId)
    {
        var ue = await FindAsync(ueId);
        var parcours = await Context.Parcours.FindAsync(parcoursId);

        if (ue != null && parcours != null)
        {
            ue.EnseigneeDans?.Add(parcours);
            await SaveChangesAsync();
        }
    }

    public async Task AssocierUeAuParcoursAsync(Ue ue,Parcours parcours)
    {
     await   AssocierUeAuParcoursAsync(ue.Id, parcours.Id);
    }


    // Dissocier une UE d'un parcours
    public async Task DissocierUeDuParcoursAsync(long ueId, long parcoursId)
    {
        var ue = await FindAsync(ueId);
        var parcours = ue?.EnseigneeDans?.FirstOrDefault(p => p.Id == parcoursId);

        if (parcours != null)
        {
            ue!.EnseigneeDans?.Remove(parcours);
            await SaveChangesAsync();
        }
    }

    // Lister les parcours associés à une UE
    public async Task<List<Parcours>> ListerParcoursAsync(long ueId)
    {
        var ue = await FindAsync(ueId);
        return ue?.EnseigneeDans ?? new List<Parcours>();
    }
    public async Task<Ue> GetByIdAsync(long id)
    {
        return await Context.Ues.FindAsync(id);
    }
}
