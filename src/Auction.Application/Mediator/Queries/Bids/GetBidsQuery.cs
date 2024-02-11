using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Domain.Common;
using Auction.Domain.Common.Dtos;

namespace Auction.Application.Mediator.Queries.Bids;

public record GetBidsQuery : QueryBase<PaginatedListDto<BidDto>>
{
    public Guid? UserId { get; init; }
    public Guid? LotId { get; init; }
    public required PaginationOptions Pagination { get; init; }
}