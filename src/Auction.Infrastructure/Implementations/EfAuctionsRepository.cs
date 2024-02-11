using System.Linq.Expressions;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Entities;
using Auction.Domain.Exceptions;
using Auction.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Implementations;

public class AuctionsRepository(AuctionDbContext dbContext) : EfRepository<AuctionEntity, AuctionDbContext>(dbContext)
{
    private readonly AuctionDbContext _dbContext = dbContext;

    public override async Task<AuctionEntity> SingleAsync(
        Expression<Func<AuctionEntity, bool>> filter,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await _dbContext
            .Auctions
            .Include(auction => auction.Lots)
            .SingleOrDefaultAsync(filter, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.For<AuctionEntity>();
        }

        return entity;
    }
}