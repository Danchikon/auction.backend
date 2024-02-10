using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Users;

public record CreateUserCommand : CommandBase<UserDto>
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}