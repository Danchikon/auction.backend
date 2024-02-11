using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Bids;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Bids;

public class CreateBidCommandHandler(
    IRepository<BidEntity> bidsRepository,
    IRepository<LotEntity> lotsRepository,
    IEventsPublisher eventsPublisher,
    IMapper mapper
    ) : CommandHandlerBase<CreateBidCommand, BidDto>
{
    public override async Task<BidDto> Handle(CreateBidCommand command, CancellationToken cancellationToken = default)
    {
        var bidId = Guid.NewGuid();
        var bidCreatedAt = DateTimeOffset.UtcNow;
        
        var bidEntity = new BidEntity
        {
            Id = bidId,
            Price = command.Price,
            LotId = command.LotId,
            UserId = command.UserId,
            CreatedAt = bidCreatedAt
        };

        var createdBid = await bidsRepository.AddAsync(bidEntity, cancellationToken);
        
        var lotEntity = await lotsRepository.SingleAsync(lot => lot.Id == command.LotId, cancellationToken);

        await eventsPublisher.PublishAsync(new EventDto<BidCreatedEventDataDto>
        {
            Channel = $"auctions.{lotEntity.AuctionId}.lots.bids.created",
            Data = new BidCreatedEventDataDto
            {
                UserId = createdBid.User!.Id,
                UserFullName = createdBid.User!.FullName,
                UserAvatar = createdBid.User!.Avatar,
                LotId = lotEntity.Id,
                AuctionId = lotEntity.AuctionId,
                Price = createdBid.Price
            }
        }, cancellationToken);

        var bidDto = mapper.Map<BidDto>(createdBid);

        return bidDto;
    }
}