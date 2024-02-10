using Auction.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Implementations;

public class EfUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork where TDbContext: DbContext
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}