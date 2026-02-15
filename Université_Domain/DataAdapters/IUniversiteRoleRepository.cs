using Université_Domain.Entities;

namespace Université_Domain.DataAdapters;

public interface IUniversiteRoleRepository : IRepository<IUniversiteRole>
{
    Task AddRoleAsync(string role);
}