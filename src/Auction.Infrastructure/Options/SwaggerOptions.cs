namespace Auction.Infrastructure.Options;

public record SwaggerOptions
{
    public const string Section = "Swagger";
    
    public required string RoutePrefix { get; init; }
    public string[] ServersUrls { get; init; } = Array.Empty<string>();
    public SwaggerEndpointOptions[] Endpoints { get; init; } = Array.Empty<SwaggerEndpointOptions>();
}