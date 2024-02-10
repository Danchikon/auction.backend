namespace Auction.Api.Dtos;

public record CreateMessageDto
{
    public required Guid AuctionId { get; init; }
    public required string Text { get; init; }
}