namespace Auction.Application.Dtos;

public record FileDto : IAsyncDisposable
{
    public required Stream Stream { get; init; }
    public string ContentType { get; init; } = "application/octet-stream";

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
};