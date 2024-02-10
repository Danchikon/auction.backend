using Auction.Application.Abstractions;
using Auction.Application.Dtos;
using Minio;
using Minio.DataModel.Args;

namespace Auction.Infrastructure.Implementations;

public class MinioFilesStorage(IMinioClient minioClient) : IFilesStorage
{
    public async Task UploadAsync(
        FileDto file, 
        string folder,
        string name, 
        CancellationToken cancellationToken = default
        )
    {
        var putObjectArgs = new PutObjectArgs()
            .WithStreamData(file.Stream)
            .WithObjectSize(file.Stream.Length)
            .WithContentType(file.ContentType)
            .WithBucket(folder)
            .WithFileName(name);
        
        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }
}