using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Lots;

public record UploadLotAvatarCommand : CommandBase<Uri>
{
    public required Guid LotId { get; set; }
    public required FileDto Avatar { get; init; }
}