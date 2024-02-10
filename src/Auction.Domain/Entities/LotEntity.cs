using Auction.Domain.Common;
using Auction.Domain.Enums;

namespace Auction.Domain.Entities;

public class LotEntity : Entity<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal StartPrice { get; set; }
    public required decimal MinPriceStepSize { get; set; }
    public required TimeSpan Duration { get; set; }
    public required LotState State { get; set; }
    public BidEntity? BestBid { get; set; }
}