using System.Linq.Expressions;
using Université_Domain.Entities;
 
namespace Université_Domain.DataAdapters;
 
public interface IEtudiantRepository :IRepository<Etudiant>
{
    public Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant);
    Task<List<Etudiant>> GetEtudiantsByUeIdAsync(long ueId);
}