using System.Linq.Expressions;
using Auction.Domain.Common;
using Auction.Domain.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Implementations;

public class EfRepository<TEntity, TDbContext>(TDbContext dbContext) : IRepository<TEntity> where TDbContext: DbContext where TEntity : Entity<Guid>
{
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entry.Entity;
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await dbContext
            .Set<TEntity>()
            .AddRangeAsync(entities, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext
            .Set<TEntity>()
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await dbContext
            .Set<TEntity>()
            .Where(entity => entity.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TDto[]> WhereAsync<TDto>(
        Expression<Func<TEntity, bool>> filter, 
        Expression<Func<TEntity, object>> sortBy,
        int limit,
        CancellationToken cancellationToken = default
        )
    {
        var dtos = await dbContext
            .Set<TEntity>()
            .Where(filter)
            .OrderByDescending(sortBy)
            .Take(limit)
            .ProjectToType<TDto>()
            .ToArrayAsync(cancellationToken);

        return dtos;
    }


    public virtual async Task<TEntity> SingleAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await dbContext
            .Set<TEntity>()
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.For<TEntity>();
        }

        return entity;
    }

    public virtual async Task<TDto> SingleAsync<TDto>(
        Expression<Func<TEntity, bool>> filter, 
        CancellationToken cancellationToken = default
        )
    {
        var entityDto = await dbContext
            .Set<TEntity>()
            .ProjectToType<TDto>()
            .SingleOrDefaultAsync(cancellationToken);

        if (entityDto is null)
        {
            throw NotFoundException.For<TEntity>();
        }

        return entityDto;
    }
}