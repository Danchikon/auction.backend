using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands;
using Auction.Application.Mediator.Commands.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Users;

public class UpdateUserCommandHandler(
    IRepository<UserEntity> usersRepository,
    IMapper mapper
    ) : CommandHandlerBase<UpdateUserCommand, UserDto>
{
    public override async Task<UserDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var userEntity = await usersRepository.SingleAsync(user => user.Id == command.Id, cancellationToken);

        userEntity.FullName = command.FullName;
        userEntity.Email = command.Email;

        var updatedUser = await usersRepository.UpdateAsync(userEntity, cancellationToken);

        var userDto = mapper.Map<UserDto>(updatedUser);

        return userDto;
    }
}