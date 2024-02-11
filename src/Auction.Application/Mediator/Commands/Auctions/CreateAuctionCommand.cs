using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Auctions;

public record CreateAuctionCommand : CommandBase<AuctionDto>
{
    public required Guid UserId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public CreateLotCommand[] Lots { get; set; } = Array.Empty<CreateLotCommand>();
}