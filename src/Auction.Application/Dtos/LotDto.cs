using Auction.Domain.Enums;

namespace Auction.Application.Dtos;

public record LotDto : EntityDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal StartPrice { get; set; }
    public required decimal MinPriceStepSize { get; set; }
    public required TimeSpan Duration { get; set; }
    public required LotState State { get; set; }
    public IEnumerable<BidDto> Bids { get; set; }
}