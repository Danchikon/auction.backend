namespace Auction.Application.Dtos;

public abstract record EntityDto
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}