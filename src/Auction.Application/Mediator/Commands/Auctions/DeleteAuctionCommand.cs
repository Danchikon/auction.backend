using Auction.Application.Common.Mediator;

namespace Auction.Application.Mediator.Commands.Auctions;

public record DeleteAuctionCommand : CommandBase
{
    public Guid Id { get; set; }
}