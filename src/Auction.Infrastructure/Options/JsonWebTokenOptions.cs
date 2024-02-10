namespace Auction.Infrastructure.Options;

public record JsonWebTokenOptions
{
    public const string Section = "JsonWebToken";
    
    public required string SecretKey { get; init; }
    public required int TokenLifetimeInMinutes { get; init; }
}