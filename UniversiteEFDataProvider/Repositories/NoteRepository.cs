using Microsoft.EntityFrameworkCore;
using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    //public NoteRepository(UniversiteDbContext context) : base(context) { }

    // Ajouter une note pour un étudiant et une UE
    public async Task AjouterNoteAsync(long etudiantId, long ueId, float valeur)
    {
        var note = new Note
        {
            EtudiantId = etudiantId,
            UeId = ueId,
            Valeur = valeur
        };

        await CreateAsync(note);
    }

    public async Task AjouterNoteAsync(Etudiant etudiant , Ue ue,  float valeur)
    {
        await AjouterNoteAsync(etudiant.Id, ue.Id, valeur);
    }
    // to implement and check later on 
    // Modifier la valeur d'une note existante
    
    public async Task ModifierValeurNoteAsync(long etudiantId, long ueId, float nouvelleValeur)
    {
        var note = await FindAsync(etudiantId, ueId);
        if (note != null)
        {
            note.Valeur = nouvelleValeur;
            await UpdateAsync(note);
        }
    }

    // Supprimer une note pour un étudiant et une UE
    public async Task SupprimerNoteAsync(long etudiantId, long ueId)
    {
        var note = await FindAsync(etudiantId, ueId);
        if (note != null)
        {
            await DeleteAsync(note);
        }
    }
    
    public async Task SaveOrUpdateAsync(Note note)
    {
        var existingNote = await context.Notes
            .FirstOrDefaultAsync(n => n.EtudiantId == note.EtudiantId && n.UeId == note.UeId);

        if (existingNote != null)
        {
            existingNote.Valeur = note.Valeur;
        }
        else
        {
            await context.Notes.AddAsync(note);
        }
        await context.SaveChangesAsync();
    }
}
