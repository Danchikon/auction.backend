using System.Linq.Expressions;
using Auction.Domain.Common.Dtos;

namespace Auction.Domain.Common;

public interface IRepository<TEntity> 
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> filter,  CancellationToken cancellationToken = default);
    Task<TDto[]> WhereAsync<TDto>(
        Expression<Func<TEntity, bool>> filter, 
        Expression<Func<TEntity, object>> sortBy,
        int limit,
        CancellationToken cancellationToken = default
        );
    
    Task<PaginatedListDto<TDto>> PaginateAsync<TDto>(
        Expression<Func<TEntity, bool>> filter, 
        Expression<Func<TEntity, object>> sortBy,
        PaginationOptions pagination,
        CancellationToken cancellationToken = default
    );
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    Task<TDto> SingleAsync<TDto>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}