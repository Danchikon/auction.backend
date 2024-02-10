using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Messages;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.QueryHandlers.Messages;

public class GetMessagesQueryHandler(
    IRepository<MessageEntity> messagesRepository
    ) : QueryHandlerBase<GetMessagesQuery, MessageDto[]>
{
    public override async Task<MessageDto[]> Handle(GetMessagesQuery query, CancellationToken cancellationToken = default)
    {
        var messagesDtos = await messagesRepository.WhereAsync<MessageDto>(
            message => true, 
            message => message.CreatedAt,
            query.Limit,
            cancellationToken
            );

        return messagesDtos;
    }
}