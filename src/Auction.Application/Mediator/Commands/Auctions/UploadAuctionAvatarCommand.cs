using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Auctions;

public record UploadAuctionAvatarCommand : CommandBase<Uri>
{
    public required Guid AuctionId { get; set; }
    public required FileDto Avatar { get; init; }
}