using System.Linq.Expressions;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Entities;
using Auction.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Implementations;

public class EfBidsRepository(AuctionDbContext dbContext) : EfRepository<BidEntity, AuctionDbContext>(dbContext)
{
    private readonly AuctionDbContext _dbContext = dbContext;

    public override async Task<BidEntity> AddAsync(BidEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext
            .Bids
            .AddAsync(entity, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await entry
            .Reference(message => message.User)
            .LoadAsync(cancellationToken);
        
        return entry.Entity;
    }
    
    public override async Task<PaginatedListDto<TDto>> PaginateAsync<TDto>(
        Expression<Func<BidEntity, bool>> filter,
        Expression<Func<BidEntity, object>> sortBy,
        PaginationOptions pagination,
        CancellationToken cancellationToken = default
    )
    {
        var queryable = _dbContext
            .Bids
            .Where(filter)
            .Include(bid => bid.User);

        var totalCount = await queryable.CountAsync(cancellationToken: cancellationToken);
        
        var items = await queryable
            .OrderByDescending(sortBy)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectToType<TDto>()
            .ToArrayAsync(cancellationToken);

        return new PaginatedListDto<TDto>
        {
            TotalCount = totalCount,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Items = items
        };
    }
}