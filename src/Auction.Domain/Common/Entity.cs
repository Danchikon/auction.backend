namespace Auction.Domain.Common;

public abstract class Entity<TId>
{
    public required TId Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}