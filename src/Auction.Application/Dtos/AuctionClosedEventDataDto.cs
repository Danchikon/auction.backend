namespace Auction.Application.Dtos;

public record AuctionClosedEventDataDto
{
    public required Guid AuctionId { get; init; }
    public required string AuctionTitle { get; init; }
    public required Uri? AuctionAvatar { get; init; }
}