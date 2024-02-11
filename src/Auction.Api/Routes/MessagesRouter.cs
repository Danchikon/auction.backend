using System.Security.Claims;
using Auction.Api.Dtos;
using Auction.Application.Mediator.Commands.Messages;
using Auction.Application.Mediator.Queries.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class MessagesRouter
{
    public static void MapMessagesRoutes(this IEndpointRouteBuilder endpoints)
    {              
        endpoints.MapGet("/",async (
            [FromQuery] int limit,
            IMediator mediator, 
            CancellationToken cancellationToken
        ) =>
        {
            var query = new GetMessagesQuery
            {
                Limit = limit
            };
            
            var messagesDtos = await mediator.Send(query, cancellationToken);
            
            return Results.Ok(messagesDtos);
        }).RequireAuthorization();
        
        endpoints.MapPost("/",async (
            [FromBody] CreateMessageDto dto,
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

            var command = new CreateMessageCommand
            {
                Text = dto.Text,
                UserId = userId,
                AuctionId = dto.AuctionId
            };
            
            var messageDto = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(messageDto);
        }).RequireAuthorization();
    }
}