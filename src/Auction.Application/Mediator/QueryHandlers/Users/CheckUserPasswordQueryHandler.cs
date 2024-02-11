using Auction.Application.Abstractions;
using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using MapsterMapper;

namespace Auction.Application.Mediator.QueryHandlers.Users;

public class CheckUserPasswordQueryHandler(
    IRepository<UserEntity> usersRepository,
    IPasswordHasher passwordHasher,
    IMapper mapper
    ) : QueryHandlerBase<CheckUserPasswordQuery, UserDto?>
{
    public override async Task<UserDto?> Handle(CheckUserPasswordQuery command, CancellationToken cancellationToken = default)
    {
        var userEntity = await usersRepository.SingleAsync(user => user.Email == command.Email, cancellationToken);

        var hash = passwordHasher.Hash(command.Password);

        return hash == userEntity.PasswordHash ? mapper.Map<UserDto>(userEntity) : null;
    }
}