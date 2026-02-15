using Université_Domain.Entities;

namespace Université_Domain.DataAdapters;

public interface IUniversiteUserRepository : IRepository<IUniversiteUser>
{
    Task<IUniversiteUser?> AddUserAsync(string login, string email, string password, string role, Etudiant? etudiant);
    Task<IUniversiteUser> FindByEmailAsync(string email);
    Task<bool> IsInRoleAsync(string email, string role);
    Task<int> DeleteAsync(long id);
    

}