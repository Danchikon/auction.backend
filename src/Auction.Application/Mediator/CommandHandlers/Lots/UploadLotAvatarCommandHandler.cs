using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Mediator.Commands.Lots;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.CommandHandlers.Lots;

public class UploadLotAvatarCommandHandler(IRepository<LotEntity> lotRepository, IFilesStorage filesStorage) : CommandHandlerBase<UploadLotAvatarCommand, Uri>
{
    public override async Task<Uri> Handle(UploadLotAvatarCommand command, CancellationToken cancellationToken = default)
    {
        var lot = await lotRepository.SingleAsync(auction => auction.Id == command.LotId, cancellationToken);

        var avatarUri = await filesStorage.UploadAsync(command.Avatar, "avatars", lot.Id.ToString(), cancellationToken);
        lot.Avatar = avatarUri;
        
        await lotRepository.UpdateAsync(lot, cancellationToken);

        return avatarUri;
    }
}