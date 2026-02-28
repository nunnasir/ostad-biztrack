using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
{
    protected readonly BizTrackDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(BizTrackDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id! }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? include = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = tracking ? DbSet.AsTracking() : DbSet.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(include))
        {
            foreach (var nav in include.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                query = query.Include(nav);
        }
        if (predicate != null)
            query = query.Where(predicate);
        if (orderBy != null)
            query = orderBy(query);
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}
