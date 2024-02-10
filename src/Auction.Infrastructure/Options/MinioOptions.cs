namespace Auction.Infrastructure.Options;

public record MinioOptions
{
    public const string Section = "Minio";
    
    public required string Endpoint { get; init; }
    public required string AccessKey { get; init; }
    public required string SecretKey { get; init; }
    public required string PublicUrl { get; init; }
}