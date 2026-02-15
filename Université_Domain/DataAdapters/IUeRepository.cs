using Université_Domain.Entities;

namespace Université_Domain.DataAdapters;

public interface IUeRepository : IRepository<Ue>
{
    Task<Ue> GetByIdAsync(long id);
}