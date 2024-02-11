using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.CommandHandlers.Auctions;

public class UploadAuctionAvatarCommandHandler(IRepository<AuctionEntity> auctionRepository, IFilesStorage filesStorage) : CommandHandlerBase<UploadAuctionAvatarCommand, Uri>
{
    public override async Task<Uri> Handle(UploadAuctionAvatarCommand command, CancellationToken cancellationToken = default)
    {
        var auction = await auctionRepository.SingleAsync(auction => auction.Id == command.AuctionId);

        var avatarUri = await filesStorage.UploadAsync(command.Avatar, "avatars", auction.Id.ToString(), cancellationToken);
        auction.Avatar = avatarUri;
        
        await auctionRepository.UpdateAsync(auction, cancellationToken);

        return avatarUri;
    }
}