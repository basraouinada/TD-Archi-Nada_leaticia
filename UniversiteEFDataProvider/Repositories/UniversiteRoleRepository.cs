using Microsoft.AspNetCore.Identity;
using Université_Domain.DataAdapters;
using Université_Domain.Entities;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Entities;

namespace UniversiteEFDataProvider.Repositories;

public class UniversiteRoleRepository(UniversiteDbContext context, RoleManager<UniversiteRole> roleManager) : Repository<IUniversiteRole>(context), IUniversiteRoleRepository
{
    public async Task AddRoleAsync(string role)
    { 
        await roleManager.CreateAsync(new UniversiteRole(role));
    }
}