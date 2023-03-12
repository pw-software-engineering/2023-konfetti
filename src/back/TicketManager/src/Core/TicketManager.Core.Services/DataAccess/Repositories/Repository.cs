using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Services.DataAccess.Repositories;

public class Repository<TEntity, TId>
    where TEntity : class, IIdentifiable<TId>
    where TId : IComparable<TId>
{
    private readonly CoreDbContext dbContext;

    public Repository(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Delete(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Add(TEntity entity)
    {
        UpdateOptimisticConcurrencyIfNecessary(entity);
        dbContext.Set<TEntity>().Add(entity);
    }
    
    public void Update(TEntity entity)
    {
        UpdateOptimisticConcurrencyIfNecessary(entity);
        dbContext.Set<TEntity>().Update(entity);
    }
    
    public void Delete(TEntity entity)
    {
        UpdateOptimisticConcurrencyIfNecessary(entity);
        dbContext.Set<TEntity>().Remove(entity);
    }

    private void UpdateOptimisticConcurrencyIfNecessary(TEntity entity)
    {
        if (entity is IOptimisticConcurrent oc)
        {
            oc.DateModified = DateTime.UtcNow;
        }
    }

    public async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity> FindAndEnsureExistenceAsync(TId id, CancellationToken cancellationToken)
    {
        var result = await FindAsync(id, cancellationToken);

        if (result is null)
        {
            throw new EntityDoesNotExistException();
        }

        return result;
    }
}
