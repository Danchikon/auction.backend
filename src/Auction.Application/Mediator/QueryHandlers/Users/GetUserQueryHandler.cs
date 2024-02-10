using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Queries.Users;
using Auction.Domain.Common;
using Auction.Domain.Entities;

namespace Auction.Application.Mediator.QueryHandlers.Users;

public class GetUserQueryHandler(IRepository<UserEntity> usersRepository) : QueryHandlerBase<GetUserQuery, UserDto>
{
    public override async Task<UserDto> Handle(GetUserQuery command, CancellationToken cancellationToken = default)
    {
        var userDto = await usersRepository.SingleAsync<UserDto>(user => user.Id == command.Id, cancellationToken);

        return userDto;
    }
}