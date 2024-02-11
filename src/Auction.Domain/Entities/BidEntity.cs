using Auction.Domain.Common;

namespace Auction.Domain.Entities;

public class BidEntity : Entity<Guid>
{
    public required decimal Price { get; init; }
    public required Guid LotId { get; init; }
    public required Guid UserId { get; init; }
    public UserEntity? User { get; init; }
}