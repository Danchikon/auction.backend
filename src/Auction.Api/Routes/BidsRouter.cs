using System.Security.Claims;
using Auction.Api.Dtos;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Bids;
using Auction.Application.Mediator.Commands.Lots;
using Auction.Application.Mediator.Queries.Bids;
using Auction.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class BidsRouter
{
    public static void MapBidsRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/",async (
                [FromQuery] Guid? lotId,
                [FromQuery] Guid? userId,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                IMediator mediator, 
                CancellationToken cancellationToken
            ) =>
            {
                var query = new GetBidsQuery
                {
                    LotId = lotId,
                    UserId = userId,
                    Pagination = new PaginationOptions
                    {
                        Page = page ?? PaginationOptions.DefaultPage,
                        PageSize = pageSize ?? PaginationOptions.DefaultPageSize
                    }
                };
                
                var bidsDtos = await mediator.Send(query, cancellationToken);
            
                return Results.Ok(bidsDtos);
            })
            .RequireAuthorization();
        
        endpoints.MapPost("/",async (
                [FromBody] CreateBidDto dto,
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
                
                var command = new CreateBidCommand
                {
                    UserId = userId,
                    LotId = dto.LotId,
                    Price = dto.Price
                };

                var bidDto = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(bidDto);
            })
            .RequireAuthorization();
    }
}