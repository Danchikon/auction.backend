using Auction.Application.Mediator.Commands.Auctions;
using Auction.Infrastructure.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class AuctionsRouter
{
    public static void MapAuctionsRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/",async (
            [FromBody] CreateAuctionCommand command,
            IMediator mediator, 
            CancellationToken cancellationToken
        ) =>
        {
            var auctionDto = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(new
            {
                Auction = auctionDto,
            });
        });
    }
}