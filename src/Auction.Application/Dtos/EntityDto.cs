namespace Auction.Application.Dtos;

public abstract record EntityDto<TId>
{
    public required TId Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}