using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Domain.Common;

namespace TicketManager.PaymentService.Services.DataAccess.Repositories;

public class Repository<TEntity, TId>
    where TEntity : class, IAggregateRoot<TId>
    where TId : IComparable<TId>
{
    private readonly PaymentDbContext dbContext;

    public Repository(PaymentDbContext dbContext)
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
    
    public virtual void Add(TEntity entity)
    {
        UpdateOptimisticConcurrencyIfNecessary(entity);
        dbContext.Set<TEntity>().Add(entity);
    }
    
    public virtual void Update(TEntity entity)
    {
        UpdateOptimisticConcurrencyIfNecessary(entity);
        dbContext.Set<TEntity>().Update(entity);
    }
    
    public virtual void Delete(TEntity entity)
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
        return await dbContext.Set<TEntity>().AsTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<TEntity> FindAndEnsureExistenceAsync(TId id, CancellationToken cancellationToken)
    {
        var result = await FindAsync(id, cancellationToken);

        if (result is null)
        {
            throw new EntityDoesNotExistException();
        }

        return result;
    }
}
