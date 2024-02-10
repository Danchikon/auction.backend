using System.Linq.Expressions;
using Auction.Domain.Common;
using Auction.Domain.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Implementations;

public class EfRepository<TEntity, TDbContext>(TDbContext dbContext) : IRepository<TEntity> where TDbContext: DbContext where TEntity : class
{
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext
            .Set<TEntity>()
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<TDto[]> WhereAsync<TDto>(
        Expression<Func<TEntity, bool>> filter, 
        Expression<Func<TEntity, object>> sortBy,
        int limit,
        CancellationToken cancellationToken = default
        )
    {
        var dtos = await dbContext
            .Set<TEntity>()
            .Where(filter)
            .Take(limit)
            .OrderBy(sortBy)
            .ProjectToType<TDto>()
            .ToArrayAsync(cancellationToken);

        return dtos;
    }


    public async Task<TEntity> SingleAsync(
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

    public async Task<TDto> SingleAsync<TDto>(
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