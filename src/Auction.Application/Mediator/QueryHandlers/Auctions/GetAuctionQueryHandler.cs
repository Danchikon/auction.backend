using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.QueryHandlers.Auctions;

public class GetAuctionQueryHandler(IRepository<AuctionEntity> auctionsRepository) : QueryHandlerBase<GetAuctionQuery, AuctionDto>
{
    public override async Task<AuctionDto> Handle(GetAuctionQuery query, CancellationToken cancellationToken = default)
    {
        var auctionDto = await auctionsRepository.SingleAsync<AuctionDto>(auction => auction.Id == query.Id, cancellationToken);

        return auctionDto;
    }
}