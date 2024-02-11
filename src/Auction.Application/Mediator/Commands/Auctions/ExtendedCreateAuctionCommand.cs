using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Auctions;

public record ExtendedCreateAuctionCommand : CreateAuctionCommand
{
    public Guid UserId { get; set; }
    public FileDto Avatar { get; set; }
}