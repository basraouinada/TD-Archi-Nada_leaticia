/*using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Université_Domain.DataAdapters;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public abstract class Repository<T>(UniversiteDbContext context) : IRepository<T>
    where T : class
{
    protected  readonly UniversiteDbContext Context = context;
    
    public async Task<T> CreateAsync(T entity)
    {
        var res = Context.Add(entity);
        await Context.SaveChangesAsync();
        return res.Entity;
    }
    public async Task UpdateAsync(T entity)
    {
        var res=Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(List<T> entities)
    {
        Context.Set<T>().UpdateRange(entities);
    }
    
    public async Task DeleteAsync(long id)
    {
        var entity = await FindAsync(id);
        
        if (entity != null)
        {
            try
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    public async Task DeleteAsync(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }
    
    // Clé primaire non composée
    public async Task<T?> FindAsync(long id)
    {
        return await Context.Set<T>().FindAsync(id);
    }
    // Clé primaire composée
    public async Task<T?> FindAsync(params object[] keyValues)
    {
        return await Context.Set<T>().FindAsync(keyValues);
    }
    
    public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> condition)
    {
        return await Context.Set<T>().Where(condition).ToListAsync();
    }

    public async Task<List<T>> FindAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }
}*/

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Université_Domain.DataAdapters;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public abstract class Repository<T>(UniversiteDbContext context) : IRepository<T>
    where T : class
{
    protected readonly UniversiteDbContext Context = context;

    public async Task<T> CreateAsync(T entity)
    {
        var res = Context.Add(entity);
        await Context.SaveChangesAsync();
        return res.Entity;
    }

    public async Task UpdateAsync(T entity)
    {
        // Check if the entity is already being tracked
        var trackedEntity = Context.ChangeTracker.Entries<T>()
                                    .FirstOrDefault(e => e.Entity.Equals(entity));

        if (trackedEntity != null)
        {
            // Detach the tracked entity to avoid conflicts
            Context.Entry(trackedEntity.Entity).State = EntityState.Detached;
        }

        // Attach the entity to the context and mark it as modified
        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(List<T> entities)
    {
        foreach (var entity in entities)
        {
            var trackedEntity = Context.ChangeTracker.Entries<T>()
                                        .FirstOrDefault(e => e.Entity.Equals(entity));

            if (trackedEntity != null)
            {
                Context.Entry(trackedEntity.Entity).State = EntityState.Detached;
            }

            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await FindAsync(id);

        if (entity != null)
        {
            try
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task DeleteAsync(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    // Clé primaire non composée
    public async Task<T?> FindAsync(long id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    // Clé primaire composée
    public async Task<T?> FindAsync(params object[] keyValues)
    {
        return await Context.Set<T>().FindAsync(keyValues);
    }

    public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> condition)
    {
        return await Context.Set<T>().Where(condition).ToListAsync();
    }

    public async Task<List<T>> FindAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }
}
