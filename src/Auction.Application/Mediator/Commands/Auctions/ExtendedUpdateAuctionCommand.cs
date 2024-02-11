namespace Auction.Application.Mediator.Commands.Auctions;

public record ExtendedUpdateAuctionCommand : UpdateAuctionCommand
{
    public Guid Id { get; set; }
}