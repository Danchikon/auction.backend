namespace Auction.Application.Dtos;

public record BidDto : EntityDto<Guid>
{
    public required decimal Price { get; init; }
    public UserDto User { get; init; }
}