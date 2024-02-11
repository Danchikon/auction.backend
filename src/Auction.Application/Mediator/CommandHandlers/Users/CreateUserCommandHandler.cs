using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Exceptions;
using MapsterMapper;

namespace Auction.Application.Mediator.CommandHandlers.Users;

public class CreateUserCommandHandler(
    IPasswordHasher passwordHasher, 
    IRepository<UserEntity> usersRepository,
    IMapper mapper
    ) : CommandHandlerBase<CreateUserCommand, UserDto>
{
    public override async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var anyAsync = await usersRepository.AnyAsync(user => user.Email == command.Email, cancellationToken);

        if (anyAsync)
        {
            throw AlreadyExistsException.For<UserEntity>();
        }
        
        var passwordHash = passwordHasher.Hash(command.Password);

        var id = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;
        
        var userEntity = new UserEntity
        {
            Id = id,
            Email = command.Email,
            FullName = command.FullName,
            PasswordHash = passwordHash,
            CreatedAt = createdAt
        };

        var updatedUser = await usersRepository.AddAsync(userEntity, cancellationToken);

        var userDto = mapper.Map<UserDto>(updatedUser);

        return userDto;
    }
}