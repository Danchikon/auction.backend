using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Auctions;

public class UpdateAuctionCommandHandler(IMapper mapper, IRepository<AuctionEntity> auctionRepository) : CommandHandlerBase<ExtendedUpdateAuctionCommand, AuctionDto>
{
    public override async Task<AuctionDto> Handle(ExtendedUpdateAuctionCommand command, CancellationToken cancellationToken = default)
    {
        var auction = await auctionRepository.SingleAsync(auction => auction.Id == command.Id);

        auction.Title = command.Title;
        auction.Description = command.Description;

        var updatedAuction =  await auctionRepository.UpdateAsync(auction, cancellationToken);

        return mapper.Map<AuctionDto>(updatedAuction);
    }
}