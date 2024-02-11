using Auction.Application.Mediator.Commands.Auctions;

namespace Auction.Api.Dtos;

public record CreateAuctionDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public CreateLotCommand[] Lots { get; set; } = Array.Empty<CreateLotCommand>();
}