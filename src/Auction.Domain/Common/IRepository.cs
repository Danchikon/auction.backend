using System.Linq.Expressions;

namespace Auction.Domain.Common;

public interface IRepository<TEntity> 
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    Task<TDto> SingleAsync<TDto>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}