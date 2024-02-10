namespace Auction.Application.Dtos;

public record MessageDto : EntityDto<Guid>
{
    public required string Text { get; init; }
    public required UserDto User { get; init; }
}