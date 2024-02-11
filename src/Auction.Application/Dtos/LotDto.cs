using Auction.Domain.Enums;

namespace Auction.Application.Dtos;

public record LotDto : EntityDto<Guid>
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required decimal StartPrice { get; init; }
    public required decimal MinPriceStepSize { get; init; }
    public required DateTimeOffset OpensAt { get; init; }
    public required DateTimeOffset ClosesAt { get; init; }
    public required LotState State { get; init; }
    public BidDto[] Bids { get; init; } = Array.Empty<BidDto>();
}