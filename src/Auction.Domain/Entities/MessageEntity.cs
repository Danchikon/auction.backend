using Auction.Domain.Common;

namespace Auction.Domain.Entities;

public class MessageEntity : Entity<Guid>
{
    public required string Text { get; init; }
    public UserEntity? User { get; init; }
    public required Guid UserId { get; init; }
}