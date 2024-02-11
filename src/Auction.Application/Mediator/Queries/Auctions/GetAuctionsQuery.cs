using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;
using Auction.Domain.Enums;

namespace Auction.Application.Mediator.Queries.Auctions;

public record GetAuctionsQuery : QueryBase<PaginatedListDto<AuctionDto>>
{
    public string? Title { get; init; }
    public AuctionState? State { get; init; }
    public required PaginationOptions Pagination { get; init; }
}