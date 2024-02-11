using Auction.Domain.Common;

namespace Auction.Domain.Entities;

public class UserEntity : Entity<Guid>
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public Uri? Avatar { get; set; }
    public required string PasswordHash { get; set; }
    public List<MessageEntity> Messages { get; init; } = new();
}