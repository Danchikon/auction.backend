using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Auctions;

public class CreateAuctionCommandHandler(IMapper maper, IUnitOfWork unitOfWork, IRepository<AuctionEntity> auctionRepository, IRepository<LotEntity> lotepository)
    : CommandHandlerBase<CreateAuctionCommand, AuctionDto>
{
    public override async Task<AuctionDto> Handle(CreateAuctionCommand command, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var createdLots = Enumerable.Empty<LotEntity>();
        
        var auctionEntity = new AuctionEntity
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            State = command.IsScheduled ? AuctionState.Scheduled : AuctionState.Opened,
            CreatedAt = DateTimeOffset.UtcNow,
            ClosedAt = command.ClosedAt,
            OpensAt = command.OpensAt,
            OpenedAt = command.OpenedAt
        };
        
        var createdAuction = await auctionRepository.AddAsync(auctionEntity, cancellationToken);

        if (command.Lots is not null && command.Lots.Any())
        {
            var lotState = command.IsScheduled ? LotState.Waiting : LotState.InSale;
                
            var lots = command.Lots.Select(lot => new LotEntity
            {
                Id = Guid.NewGuid(),
                Title = lot.Title,
                Description = lot.Description,
                StartPrice = lot.StartPrice,
                MinPriceStepSize = lot.MinPriceStepSize,
                Duration = lot.Duration,
                State = lotState,
                CreatedAt = DateTimeOffset.UtcNow,
                AuctionId = createdAuction.Id
            });
            createdLots = await lotepository.AddRangeAsync(lots, cancellationToken);
        }

        var auctionDto = maper.Map<AuctionDto>(createdAuction);

        if (createdLots.Any())
        {
            auctionDto.Lots =  maper.Map<IEnumerable<LotDto>>(createdLots);
        }
        
        await unitOfWork.CommitTransactionAsync(cancellationToken);

        return auctionDto;
    }
}