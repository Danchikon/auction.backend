namespace Auction.Application.Dtos;

public record EventDto<TData>
{
    public required string Channel { get; init; }
    public required TData Data { get; init; }
};