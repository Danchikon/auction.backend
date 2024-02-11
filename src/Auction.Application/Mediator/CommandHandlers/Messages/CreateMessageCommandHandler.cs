using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Messages;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Exceptions;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Messages;

public class CreateMessageCommandHandler(
    IEventsPublisher eventsPublisher,
    IRepository<MessageEntity> messagesRepository,
    IRepository<UserEntity> usersRepository,
    IRepository<AuctionEntity> auctionsRepository,
    IMapper mapper
    ) : CommandHandlerBase<CreateMessageCommand, MessageDto>
{
    public override async Task<MessageDto> Handle(CreateMessageCommand command, CancellationToken cancellationToken = default)
    {
        var anyAuction = await auctionsRepository.AnyAsync(auction => auction.Id == command.AuctionId, cancellationToken);

        if (anyAuction is false)
        {
            throw NotFoundException.For<AuctionEntity>();
        }
        
        var userEntity = await usersRepository.SingleAsync(user => user.Id == command.UserId, cancellationToken);
        
        var messageId = Guid.NewGuid();
        var messageCreatedAt = DateTimeOffset.UtcNow;
        
        var messageEntity = new MessageEntity
        {
            Id = messageId,
            AuctionId = command.AuctionId,
            UserId = userEntity.Id,
            Text = command.Text,
            CreatedAt = messageCreatedAt
        };

        var updatedMessage = await messagesRepository.AddAsync(messageEntity, cancellationToken);

        var messageEventData = new MessageEventDataDto
        {
            Text = command.Text,
            UserFullName = userEntity.FullName,
            UserId = command.UserId,
            UserAvatar = userEntity.Avatar
        };

        var eventDto = new EventDto<MessageEventDataDto>
        {
            Channel = $"auctions.{command.AuctionId}.messages.sent",
            Data = messageEventData,
        };

        await eventsPublisher.PublishAsync(eventDto, cancellationToken);
        
        var messageDto = mapper.Map<MessageDto>(updatedMessage);

        return messageDto;
    }
}