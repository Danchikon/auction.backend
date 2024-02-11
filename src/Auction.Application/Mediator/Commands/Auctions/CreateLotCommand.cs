using Auction.Domain.Enums;

namespace Auction.Application.Mediator.Commands.Auctions;

public record CreateLotCommand
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal StartPrice { get; set; }
    public required decimal MinPriceStepSize { get; set; }
    public required DateTimeOffset OpensAt { get; set; }
    public required DateTimeOffset ClosesAt { get; set; }
}