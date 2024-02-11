using Auction.Application.Common.Mediator;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.CommandHandlers.Auctions;

public class DeleteAuctionCommandHandler(IRepository<AuctionEntity> auctionRepository) : CommandHandlerBase<DeleteAuctionCommand>
{
    public override async Task Handle(DeleteAuctionCommand command, CancellationToken cancellationToken = default)
    {
        await auctionRepository.DeleteAsync(auction => auction.Id == command.Id, cancellationToken);
    }
}