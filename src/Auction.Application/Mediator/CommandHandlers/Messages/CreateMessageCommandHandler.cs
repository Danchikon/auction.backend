using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Messages;
using Auction.Application.Mediator.Commands.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Messages;

public class CreateMessageCommandHandler(
    IEventsPublisher eventsPublisher,
    IRepository<MessageEntity> messagesRepository,
    IRepository<UserEntity> usersRepository,
    IMapper mapper
    ) : CommandHandlerBase<CreateMessageCommand, MessageDto>
{
    public override async Task<MessageDto> Handle(CreateMessageCommand command, CancellationToken cancellationToken = default)
    {
        var userEntity = await usersRepository.SingleAsync(user => user.Id == command.UserId, cancellationToken);
        
        var messageId = Guid.NewGuid();
        var messageCreatedAt = DateTimeOffset.UtcNow;
        
        var messageEntity = new MessageEntity
        {
            Id = messageId,
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
            Channel = "messages",
            Data = messageEventData,
        };

        await eventsPublisher.PublishAsync(eventDto, cancellationToken);
        
        var messageDto = mapper.Map<MessageDto>(updatedMessage);

        return messageDto;
    }
}