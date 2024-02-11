using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Bids;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.QueryHandlers.Bids;

public class GetBidsQueryHandler(IRepository<BidEntity> bidsRepository) : QueryHandlerBase<GetBidsQuery, PaginatedListDto<BidDto>>
{
    public override async Task<PaginatedListDto<BidDto>> Handle(GetBidsQuery query, CancellationToken cancellationToken = default)
    {
        var bidsDtos =  await bidsRepository.PaginateAsync<BidDto>(
            filter: bid => (query.LotId == null || query.LotId == bid.LotId) 
                           && (query.UserId == null || query.UserId == bid.UserId),
            sortBy: auction => auction.CreatedAt,
            pagination: query.Pagination,
            cancellationToken: cancellationToken
        );

        return bidsDtos;
    }
}