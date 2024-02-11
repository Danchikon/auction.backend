namespace Auction.Application.Dtos;

public record BidDto : EntityDto<Guid>
{
    public required decimal Price { get; init; }
    public required Guid LotId { get; init; }
    public required UserDto User { get; init; }
}