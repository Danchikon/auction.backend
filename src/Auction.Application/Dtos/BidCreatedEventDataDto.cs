namespace Auction.Application.Dtos;

public record BidCreatedEventDataDto
{
    public required string UserFullName { get; init; }
    public required Guid UserId { get; init; }
    public Uri? UserAvatar { get; init; }
    public required Guid AuctionId { get; init; }
    public required Guid LotId { get; init; }
    public required decimal Price { get; init; }
}