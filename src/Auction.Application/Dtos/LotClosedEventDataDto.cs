namespace Auction.Application.Dtos;

public record LotClosedEventDataDto
{
    public required Guid LotId { get; init; }
    public required string LotTitle { get; init; }
    public required Guid AuctionId { get; init; }
    public required string AuctionTitle { get; init; }
}