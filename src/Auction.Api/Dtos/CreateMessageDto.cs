namespace Auction.Api.Dtos;

public record CreateMessageDto
{
    public required string Text { get; init; }
}