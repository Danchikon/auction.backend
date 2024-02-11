using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Auctions;

public record UpdateAuctionCommand : CommandBase<AuctionDto>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}