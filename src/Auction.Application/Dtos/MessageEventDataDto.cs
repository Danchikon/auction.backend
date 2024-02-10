namespace Auction.Application.Dtos;

public record MessageEventDataDto
{
    public required string Text { get; init; }
    public required Guid UserId { get; init; }
    public required string UserFullName { get; init; }
    public Uri? UserAvatar { get; init; }
}