using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Users;
using Auction.Application.Mediator.Queries.Users;
using Auction.Infrastructure.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class UsersRouter
{
    public static void MapUsersRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/{id:guid}",async (
            [FromRoute] Guid id, 
            IMediator mediator, 
            CancellationToken cancellationToken
            ) =>
        {
            var query = new GetUserQuery { Id = id };

            var userDto = await mediator.Send(query, cancellationToken);
            
            return Results.Ok(userDto);
        }).RequireAuthorization();
        
        endpoints.MapPost("/sign-up",async (
            [FromBody] CreateUserCommand command,
            IMediator mediator, 
            JsonWebTokenService jsonWebTokenService,
            CancellationToken cancellationToken
        ) =>
        {
            var userDto = await mediator.Send(command, cancellationToken);

            var token = jsonWebTokenService.Create(new Dictionary<string, object>
            {
                ["sub"] =  userDto.Id
            });
            
            return Results.Ok(new
            {
                User = userDto,
                AccessToken = token
            });
        });
        
        endpoints.MapPost("/sign-in",async (
            [FromBody] CheckUserPasswordQuery query,
            IMediator mediator, 
            JsonWebTokenService jsonWebTokenService,
            CancellationToken cancellationToken
        ) =>
        {
            var userId = await mediator.Send(query, cancellationToken);

            if (userId is null)
            {
                return Results.Unauthorized();
            }
            
            var token = jsonWebTokenService.Create(new Dictionary<string, object>
            {
                ["sub"] =  userId
            });
            
            return Results.Ok(new
            {
                AccessToken = token
            });
        });
        
        endpoints.MapPost("/{id:guid}/avatar",async (
            [FromRoute] Guid id, 
            [FromForm] IFormFile avatar,
            IMediator mediator,   
            CancellationToken cancellationToken
        ) =>
        {
            await using var fileStream = avatar.OpenReadStream();
            
            var fileDto = new FileDto
            {
                Stream = fileStream,
                ContentType = avatar.ContentType
            };
            
            var command = new UploadUserAvatarCommand
            {
                UserId = id,
                Avatar = fileDto
            };

            var uri = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(uri);
        })
        .DisableAntiforgery()
        .RequireAuthorization();
    }
}