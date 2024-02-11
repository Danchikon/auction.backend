namespace Auction.Api.Dtos;

public record CreateBidDto
{
    public required decimal Price { get; init; }
    public required Guid LotId { get; init; }
}