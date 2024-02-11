using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.QueryHandlers.Auctions;

public class GetAuctionsQueryHandler(IRepository<AuctionEntity> auctionsRepository) : QueryHandlerBase<GetAuctionsQuery, PaginatedListDto<AuctionDto>>
{
    public override async Task<PaginatedListDto<AuctionDto>> Handle(GetAuctionsQuery query, CancellationToken cancellationToken = default)
    {
        var auctionsDtos =  await auctionsRepository.PaginateAsync<AuctionDto>(
            filter: auction => (query.State == null || query.State == auction.State) 
                               && (query.Title == null || auction.Title.ToLower().Contains(query.Title.ToLower())),
            sortBy: auction => auction.OpensAt,
            pagination: query.Pagination,
            cancellationToken: cancellationToken
            );

        return auctionsDtos;
    }
}