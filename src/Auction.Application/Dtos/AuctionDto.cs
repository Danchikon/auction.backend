using Auction.Domain.Enums;

namespace Auction.Application.Dtos;

public record AuctionDto : EntityDto<Guid>
{
    public required string Title { get; init; }
    public required string? Description { get; init; }
    public required AuctionState State { get; init; }
    public DateTimeOffset? OpensAt { get; init; }
    public DateTimeOffset? ClosesAt { get; init; }
    public LotDto[] Lots { get; init; } = Array.Empty<LotDto>();
}