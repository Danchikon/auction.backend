using Auction.Application.Common.Mediator;
using Auction.Application.Dtos;

namespace Auction.Application.Mediator.Commands.Users;

public record UploadUserAvatarCommand : CommandBase<Uri>
{
    public required Guid UserId { get; init; }
    public required FileDto Avatar { get; init; }
}