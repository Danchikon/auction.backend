using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Users;
using Auction.Application.Mediator.Queries.Users;
using Auction.Infrastructure.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class MessagesRouter
{
    public static void MapMessagesRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/",async (
            [FromBody] CreateUserCommand command,
            IMediator mediator, 
            JsonWebTokenService jsonWebTokenService,
            CancellationToken cancellationToken
        ) =>
        {
            var userDto = await mediator.Send(command, cancellationToken);

            var token = jsonWebTokenService.Create();
            
            return Results.Ok(new
            {
                User = userDto,
                AccessToken = token
            });
        });
    }
}