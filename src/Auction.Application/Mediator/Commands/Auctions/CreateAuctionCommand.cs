using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Auctions;

public record CreateAuctionCommand : CommandBase<AuctionDto>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsScheduled { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }

    public IEnumerable<CreateLotCommand>? Lots { get; set; }
}