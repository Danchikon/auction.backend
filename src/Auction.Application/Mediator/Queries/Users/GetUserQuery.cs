using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Queries.Users;

public record GetUserQuery : QueryBase<UserDto>
{
    public required Guid Id { get; init; }
}