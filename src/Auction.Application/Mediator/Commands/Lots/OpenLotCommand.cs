using Auction.Application.Common.Mediator;

namespace Auction.Application.Mediator.Commands.Lots;

public record OpenLotCommand : CommandBase
{
    public required Guid LotId { get; init; }
}