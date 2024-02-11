using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Enums;

namespace Auction.Application.Mediator.Queries.Auctions;

public record GetAuctionQuery : QueryBase<AuctionDto>
{
    public required Guid Id { get; init; }
}