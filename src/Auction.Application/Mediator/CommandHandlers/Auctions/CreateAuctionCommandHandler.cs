using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Auctions;

public class CreateAuctionCommandHandler(
    IMapper mapper, 
    IRepository<AuctionEntity> auctionsRepository,
    ILotJobsScheduler lotJobsScheduler
    )
    : CommandHandlerBase<CreateAuctionCommand, AuctionDto>
{
    public override async Task<AuctionDto> Handle(CreateAuctionCommand command, CancellationToken cancellationToken = default)
    {
        var auctionId = Guid.NewGuid();
        
        var lots = command.Lots.Select(createLotCommand => new LotEntity
        {
            Id = Guid.NewGuid(),
            Title = createLotCommand.Title,
            Description = createLotCommand.Description,
            StartPrice = createLotCommand.StartPrice,
            MinPriceStepSize = createLotCommand.MinPriceStepSize,
            OpensAt = createLotCommand.OpensAt,
            ClosesAt = createLotCommand.ClosesAt,
            State = LotState.Scheduled,
            CreatedAt = DateTimeOffset.UtcNow,
            AuctionId = auctionId
        }).ToList();

        var auctionOpensAt = lots
            .Select(lot => lot.OpensAt)
            .Order()
            .First();
        
        var auctionClosesAt = lots
            .Select(lot => lot.ClosesAt)
            .OrderDescending()
            .First();
        
        var auctionEntity = new AuctionEntity
        {
            Id = auctionId,
            Title = command.Title,
            Description = command.Description,
            State = AuctionState.Scheduled,
            CreatedAt = DateTimeOffset.UtcNow,
            OpensAt = auctionOpensAt,
            ClosesAt = auctionClosesAt,
            UserId = command.UserId,
            Lots = lots
        };
        
        var createdAuction = await auctionsRepository.AddAsync(auctionEntity, cancellationToken);
        
        foreach (var lotEntity in createdAuction.Lots)
        {
            await lotJobsScheduler.ScheduleOpenLotAsync(lotEntity.Id, lotEntity.OpensAt);
            await lotJobsScheduler.ScheduleCloseLotAsync(lotEntity.Id, lotEntity.ClosesAt);
        }

        var auctionDto = mapper.Map<AuctionDto>(createdAuction);
        
        return auctionDto;
    }
}