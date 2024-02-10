namespace Auction.Application.Dtos;

public record MessageUserDto 
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public Uri? Avatar { get; init; }
}