using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Lots;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Enums;

namespace Auction.Application.Mediator.CommandHandlers.Lots;

public class CloseLotCommandHandler(
    IRepository<LotEntity> lotsRepository,
    IRepository<AuctionEntity> auctionsRepository,
    IEventsPublisher eventsPublisher
    ) : CommandHandlerBase<CloseLotCommand>
{
    public override async Task Handle(CloseLotCommand command, CancellationToken cancellationToken = default)
    {
        var lotEntity = await lotsRepository.SingleAsync(lot => lot.Id == command.LotId, cancellationToken);
        var auctionEntity = await auctionsRepository.SingleAsync(auction => auction.Id == lotEntity.AuctionId, cancellationToken);

        if (auctionEntity.Lots.All(lot => lot.State is LotState.BidsAreClosed || lot.Id == lotEntity.Id))
        {
            auctionEntity.State = AuctionState.Closed;
            auctionEntity.Lots.Clear();
            
            await auctionsRepository.UpdateAsync(auctionEntity, cancellationToken);
            
            await eventsPublisher.PublishAsync(new EventDto<AuctionClosedEventDataDto>
            {
                Channel = "auctions.closed",
                Data = new AuctionClosedEventDataDto
                {
                    AuctionId = auctionEntity.Id,
                    AuctionTitle = auctionEntity.Title,
                    AuctionAvatar = auctionEntity.Avatar
                }
            }, cancellationToken);
        }
        
        lotEntity.State = LotState.BidsAreClosed;

        await lotsRepository.UpdateAsync(lotEntity, cancellationToken);

        await eventsPublisher.PublishAsync(new EventDto<LotClosedEventDataDto>
        {
            Channel = $"auctions.{auctionEntity.Id}.lots.closed",
            Data = new LotClosedEventDataDto
            {
                LotId = lotEntity.Id,
                LotTitle = lotEntity.Title,
                AuctionId = auctionEntity.Id,
                AuctionTitle = auctionEntity.Title
            }
        }, cancellationToken);
    }
}