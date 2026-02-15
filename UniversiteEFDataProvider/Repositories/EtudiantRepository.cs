using Microsoft.EntityFrameworkCore;
using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class EtudiantRepository(UniversiteDbContext context) : Repository<Etudiant>(context), IEtudiantRepository
{
    public async Task AffecterParcoursAsync(long idEtudiant, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        e.ParcoursSuivi = p;
        await Context.SaveChangesAsync();
    }
    
    public async Task AffecterParcoursAsync(Etudiant etudiant, Parcours parcours)
    {
        await AffecterParcoursAsync(etudiant.Id, parcours.Id); 
    }
    public async Task AjouterNoteAsync(long idEtudiant, Note note)
    {
        var etudiant = await FindAsync(idEtudiant);
        if (etudiant != null)
        {
            etudiant.Notes?.Add(note);
            await SaveChangesAsync();
        }
    }

    public async Task AjouterNoteAsync(Etudiant etudiant, Note note)
    {
        await AjouterNoteAsync(etudiant.Id, note);
    }
    
    public async Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        return await Context.Etudiants.Include(e => e.Notes).ThenInclude(n=>n.Ue).FirstOrDefaultAsync(e => e.Id == idEtudiant);
    }
    public async Task<List<Etudiant>> GetEtudiantsByUeIdAsync(long ueId)
    {
        return await Context.Etudiants
            .Where(e => Context.Notes.Any(n => n.EtudiantId == e.Id && n.UeId == ueId))
            .ToListAsync();
    }

}