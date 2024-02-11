using Auction.Domain.Common;
using Auction.Domain.Enums;

namespace Auction.Domain.Entities;

public class LotEntity : Entity<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal StartPrice { get; set; }
    public required decimal MinPriceStepSize { get; set; }
    public required DateTimeOffset OpensAt { get; set; }
    public required DateTimeOffset ClosesAt { get; set; }
    public required LotState State { get; set; }
    public IEnumerable<BidEntity> Bids { get; set; } = new List<BidEntity>();

    public Guid AuctionId { get; set; }
    public AuctionEntity Auction { get; set; }
    public Uri? Avatar { get; set; }
}