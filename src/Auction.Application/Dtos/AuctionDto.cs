using Auction.Domain.Enums;

namespace Auction.Application.Dtos;

public record AuctionDto : EntityDto<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public AuctionState State { get; set; }
    public DateTimeOffset? OpensAt { get; set; }
    public DateTimeOffset? OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }

    public IEnumerable<LotDto> Lots { get; set; }
}