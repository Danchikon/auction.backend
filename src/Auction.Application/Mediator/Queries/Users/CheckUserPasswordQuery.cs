using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Queries.Users;

public record CheckUserPasswordQuery : QueryBase<UserDto?>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}