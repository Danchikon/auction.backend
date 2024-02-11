namespace Auction.Infrastructure.Options;

public record SwaggerEndpointOptions
{
    public required string Url { get; init; }
    public required string Name { get; init; }
}