using Auction.Domain.Common;
using Auction.Domain.Enums;

namespace Auction.Domain.Entities;

public class AuctionEntity : Entity<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public AuctionState State { get; set; }
    public required DateTimeOffset OpensAt { get; set; }
    public required DateTimeOffset ClosesAt { get; set; }
    public required Guid UserId { get; init; }
    public UserEntity? User { get; init; }
    public Uri? Avatar { get; set; }
    public List<LotEntity> Lots { get; init; } = new ();
}