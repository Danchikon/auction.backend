using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Lots;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Enums;

namespace Auction.Application.Mediator.CommandHandlers.Lots;

public class OpenLotCommandHandler(
    IRepository<LotEntity> lotsRepository,
    IRepository<AuctionEntity> auctionsRepository,
    IEventsPublisher eventsPublisher
    ) : CommandHandlerBase<OpenLotCommand>
{
    public override async Task Handle(OpenLotCommand command, CancellationToken cancellationToken = default)
    {
        var lotEntity = await lotsRepository.SingleAsync(lot => lot.Id == command.LotId, cancellationToken);
        var auctionEntity = await auctionsRepository.SingleAsync(auction => auction.Id == lotEntity.AuctionId, cancellationToken);

        if (auctionEntity.Lots.All(lot => lot.State is LotState.Scheduled))
        {
            auctionEntity.State = AuctionState.Opened;
            auctionEntity.Lots.Clear();
            
            await auctionsRepository.UpdateAsync(auctionEntity, cancellationToken);
            
            await eventsPublisher.PublishAsync(new EventDto<AuctionOpenedEventDataDto>
            {
                Channel = "auctions.opened",
                Data = new AuctionOpenedEventDataDto
                {
                    AuctionId = auctionEntity.Id,
                    AuctionTitle = auctionEntity.Title,
                    AuctionAvatar = auctionEntity.Avatar
                }
            }, cancellationToken);
        }
        
        lotEntity.State = LotState.BidsAreOpened;

        await lotsRepository.UpdateAsync(lotEntity, cancellationToken);

        await eventsPublisher.PublishAsync(new EventDto<LotOpenedEventDataDto>
        {
            Channel = $"auctions.{auctionEntity.Id}.lots.opened",
            Data = new LotOpenedEventDataDto
            {
                LotId = lotEntity.Id,
                LotTitle = lotEntity.Title,
                AuctionId = auctionEntity.Id,
                AuctionTitle = auctionEntity.Title
            }
        }, cancellationToken);
    }
}