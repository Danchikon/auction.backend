using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Messages;

public record CreateMessageCommand : CommandBase<MessageDto>
{
    public required string Text { get; init; }
    public required Guid UserId { get; init; }
    public required Guid AuctionId { get; init; }
}