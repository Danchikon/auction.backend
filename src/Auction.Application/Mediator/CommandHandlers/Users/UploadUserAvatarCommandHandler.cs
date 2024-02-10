using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Mediator.Commands.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.CommandHandlers.Users;

public class UploadUserAvatarCommandHandler(
    IRepository<UserEntity> usersRepository,
    IFilesStorage filesStorage
    ) : CommandHandlerBase<UploadUserAvatarCommand, Uri>
{
    public override async Task<Uri> Handle(UploadUserAvatarCommand command, CancellationToken cancellationToken = default)
    {
        var userEntity = await usersRepository.SingleAsync(user => user.Id == command.UserId, cancellationToken);
        
        var fileName = Guid.NewGuid().ToString();
        var fileUri = new Uri($"images/{fileName}");

        userEntity.Avatar = fileUri;

        await usersRepository.UpdateAsync(userEntity, cancellationToken);
        await filesStorage.UploadAsync(command.Avatar, "images", fileName, cancellationToken);

        return fileUri;
    }
}