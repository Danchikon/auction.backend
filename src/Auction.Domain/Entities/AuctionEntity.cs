using Auction.Domain.Common;
using Auction.Domain.Enums;

namespace Auction.Domain.Entities;

public class AuctionEntity : Entity<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public AuctionState State { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public List<LotEntity> Lots { get; init; } = new ();
}