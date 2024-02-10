using Auction.Application.Dtos;

namespace Auction.Application.Abstractions;

public interface IFilesStorage
{
    Task<Uri> UploadAsync(
        FileDto file,
        string folder,
        string name,
        CancellationToken cancellationToken = default
        );
    
    Task RemoveAsync(
        string folder,
        string name,
        CancellationToken cancellationToken = default
    );
}