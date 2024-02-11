using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Bids;

public record CreateBidCommand : CommandBase<BidDto>
{
    public required decimal Price { get; init; }
    public required Guid LotId { get; init; }
    public required Guid UserId { get; init; }
}