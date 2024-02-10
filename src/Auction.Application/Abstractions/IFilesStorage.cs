using Auction.Application.Dtos;

namespace Auction.Application.Abstractions;

public interface IFilesStorage
{
    Task UploadAsync(
        FileDto file,
        string folder,
        string name,
        CancellationToken cancellationToken = default
        );
}