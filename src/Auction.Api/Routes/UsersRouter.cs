using System.Security.Claims;
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
        endpoints.MapGet("/me",async (
            IMediator mediator, 
            HttpContext httpContext,
            CancellationToken cancellationToken
            ) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
            {
                return Results.Forbid();
            }

            var userId = Guid.Parse(userIdString);
            
            var query = new GetUserQuery { Id = userId };
            
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
        
        endpoints.MapPost("/me/avatar",async (
            [FromForm] IFormFile avatar,
            IMediator mediator,   
            HttpContext httpContext,
            CancellationToken cancellationToken
        ) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null)
            {
                return Results.Forbid();
            }

            var userId = Guid.Parse(userIdString);
            
            await using var fileStream = avatar.OpenReadStream();
            
            var fileDto = new FileDto
            {
                Stream = fileStream,
                ContentType = avatar.ContentType
            };
            
            var command = new UploadUserAvatarCommand
            {
                UserId = userId,
                Avatar = fileDto
            };

            var uri = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(uri);
        })
        .DisableAntiforgery()
        .RequireAuthorization();
    }
}