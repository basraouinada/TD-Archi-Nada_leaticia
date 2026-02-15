using Microsoft.EntityFrameworkCore;
using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{

    // Ajouter un étudiant à un parcours
    public async Task AjouterEtudiantAsync(long parcoursId, long etudiantId)
    {
        var parcours = await FindAsync(parcoursId);
        var etudiant = await Context.Etudiants.FindAsync(etudiantId);

        if (parcours != null && etudiant != null)
        {
            parcours.Inscrits?.Add(etudiant);
            await Context.SaveChangesAsync();
        }
    }
    public async Task AjouterEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        await  AjouterEtudiantAsync(parcours.Id, etudiant.Id);
    }

    // Ajouter une UE à un parcours
    public async Task AjouterUeAsync(long parcoursId, long ueId)
    {
        var parcours = await FindAsync(parcoursId);
        var ue = await Context.Ues.FindAsync(ueId);

        if (parcours != null && ue != null)
        {
            parcours.UesEnseignees?.Add(ue);
            await Context.SaveChangesAsync();
        }
    }

    public async Task AjouterUeAsync(Parcours parcours, Ue ue)
    {
        await  AjouterUeAsync(parcours.Id, ue.Id);
    }


    // Supprimer une UE d'un parcours
    public async Task SupprimerUeAsync(long parcoursId, long ueId)
    {
        var parcours = await FindAsync(parcoursId);
        var ue = parcours?.UesEnseignees?.FirstOrDefault(u => u.Id == ueId);

        if (ue != null)
        {
            parcours!.UesEnseignees?.Remove(ue);
            await Context.SaveChangesAsync();
        }
    }

    // Lister les étudiants inscrits dans un parcours
    public async Task<List<Etudiant>> ListerEtudiantsAsync(long parcoursId)
    {
        var parcours = await FindAsync(parcoursId);
        return parcours?.Inscrits ?? new List<Etudiant>();
    }
    // méthodes implementer pour surpacer l"erreur 
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
        {
            if (parcours == null)
                throw new ArgumentNullException(nameof(parcours));
            if (etudiant == null)
                throw new ArgumentNullException(nameof(etudiant));

            parcours.Inscrits ??= new List<Etudiant>();
            parcours.Inscrits.Add(etudiant);
            await Context.SaveChangesAsync();
            return parcours;
        }

        // Ajouter un étudiant à un parcours en utilisant des IDs
        public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
        {
            var parcours = await FindAsync(idParcours);
            if (parcours == null)
                throw new KeyNotFoundException($"Parcours avec l'ID {idParcours} introuvable.");

            var etudiant = await Context.Etudiants.FindAsync(idEtudiant);
            if (etudiant == null)
                throw new KeyNotFoundException($"Étudiant avec l'ID {idEtudiant} introuvable.");

            parcours.Inscrits ??= new List<Etudiant>();
            parcours.Inscrits.Add(etudiant);
            await Context.SaveChangesAsync();
            return parcours;
        }

        // Ajouter une liste d'étudiants à un parcours en utilisant des objets
        public async Task<Parcours> AddEtudiantAsync(Parcours? parcours, List<Etudiant> etudiants)
        {
            if (parcours == null)
                throw new ArgumentNullException(nameof(parcours));
            if (etudiants == null || !etudiants.Any())
                throw new ArgumentException("La liste des étudiants ne peut pas être vide.", nameof(etudiants));

            parcours.Inscrits ??= new List<Etudiant>();
            foreach (var etudiant in etudiants)
            {
                parcours.Inscrits.Add(etudiant);
            }
            await Context.SaveChangesAsync();
            return parcours;
        }

        // Ajouter une liste d'étudiants à un parcours en utilisant des IDs
        public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
        {
            var parcours = await FindAsync(idParcours);
            if (parcours == null)
                throw new KeyNotFoundException($"Parcours avec l'ID {idParcours} introuvable.");

            var etudiants = await Context.Etudiants.Where(e => idEtudiants.Contains(e.Id)).ToListAsync();
            if (etudiants.Count != idEtudiants.Length)
                throw new KeyNotFoundException("Un ou plusieurs étudiants n'ont pas été trouvés.");

            parcours.Inscrits ??= new List<Etudiant>();
            foreach (var etudiant in etudiants)
            {
                parcours.Inscrits.Add(etudiant);
            }
            await Context.SaveChangesAsync();
            return parcours;
        }

        // Ajouter une UE à un parcours en utilisant des IDs
        public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
        {
            var parcours = await FindAsync(idParcours);
            if (parcours == null)
                throw new KeyNotFoundException($"Parcours avec l'ID {idParcours} introuvable.");

            var ue = await Context.Ues.FindAsync(idUe);
            if (ue == null)
                throw new KeyNotFoundException($"UE avec l'ID {idUe} introuvable.");

            parcours.UesEnseignees ??= new List<Ue>();
            parcours.UesEnseignees.Add(ue);
            await Context.SaveChangesAsync();
            return parcours;
        }

        // Ajouter une liste d'UEs à un parcours en utilisant des IDs
        public async Task<Parcours> AddUeAsync(long idParcours, long[] idUe)
        {
            var parcours = await FindAsync(idParcours);
            if (parcours == null)
                throw new KeyNotFoundException($"Parcours avec l'ID {idParcours} introuvable.");

            var ues = await Context.Ues.Where(u => idUe.Contains(u.Id)).ToListAsync();
            if (ues.Count != idUe.Length)
                throw new KeyNotFoundException("Une ou plusieurs UEs n'ont pas été trouvées.");

            parcours.UesEnseignees ??= new List<Ue>();
            foreach (var ue in ues)
            {
                parcours.UesEnseignees.Add(ue);
            }
            await Context.SaveChangesAsync();
            return parcours;
        }
}
