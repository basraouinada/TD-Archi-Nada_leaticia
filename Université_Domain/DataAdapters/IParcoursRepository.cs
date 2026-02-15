using Université_Domain.Entities;

namespace Université_Domain.DataAdapters;

public interface IParcoursRepository : IRepository<Parcours>
{
    Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant);
    Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant);
    Task<Parcours> AddEtudiantAsync(Parcours ? parcours, List<Etudiant> etudiants);
    Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants);

    Task<Parcours> AddUeAsync(long idParcours, long idUe);
    Task<Parcours> AddUeAsync(long idParcours, long [] idUe);
}