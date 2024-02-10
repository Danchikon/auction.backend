using Auction.Application.Abstractions;
using Auction.Application.Dtos;
using Auction.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Auction.Infrastructure.Implementations;

public class MinioFilesStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> options
    ) : IFilesStorage
{
    public async Task<Uri> UploadAsync(
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
            .WithObject(name);
        
        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        var uri = new Uri($"{options.Value.PublicUrl}/{folder}/{name}");

        return uri;
    }

    public async Task RemoveAsync(
        string folder, 
        string name, 
        CancellationToken cancellationToken = default
        )
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(folder)
            .WithObject(name);
        
        await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}