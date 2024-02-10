using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Queries.Messages;

public record GetMessagesQuery : QueryBase<MessageDto[]>
{
    public int Limit { get; init; } = 50;
}