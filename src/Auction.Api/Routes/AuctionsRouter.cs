using System.Security.Claims;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class AuctionsRouter
{
    public static void MapAuctionsRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/",async (
                [FromForm] IFormFile auctionAvatar,
            [FromForm] CreateAuctionCommand command,
            IMediator mediator, 
            HttpContext httpContext,
            IMapper mapper,
            CancellationToken cancellationToken
        ) =>
        {
            await using var fileStream = auctionAvatar.OpenReadStream();
            
            var file = new FileDto
            {
                Stream = fileStream,
                ContentType = auctionAvatar.ContentType
            };

            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var extendedCommand = mapper.Map<ExtendedCreateAuctionCommand>(command);
            extendedCommand.UserId = Guid.Parse(userIdString);
            extendedCommand.Avatar = file;
                
                
            var auctionDto = await mediator.Send(extendedCommand, cancellationToken);
            
            return Results.Ok(new
            {
                Auction = auctionDto,
            });
        })
            .DisableAntiforgery()
            .RequireAuthorization();
        
        endpoints.MapPatch("/{id}",async (
                [FromBody] UpdateAuctionCommand command,
                Guid id,
                IMediator mediator, 
                IMapper mapper,
                CancellationToken cancellationToken
            ) =>
            {
                var extendedCommand = mapper.Map<ExtendedUpdateAuctionCommand>(command);
                extendedCommand.Id = id;
                
                var auctionDto = await mediator.Send(extendedCommand, cancellationToken);
            
                return Results.Ok(new
                {
                    Auction = auctionDto,
                });
            })
            .DisableAntiforgery()
            .RequireAuthorization();
        
        endpoints.MapPost("/{id}/avatar",async (
                [FromForm] IFormFile avatar,
                Guid id,
                IMediator mediator,   
                HttpContext httpContext,
                CancellationToken cancellationToken
            ) =>
            {
                var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString is null)
                    return Results.Forbid();
                
                await using var fileStream = avatar.OpenReadStream();
            
                var fileDto = new FileDto
                {
                    Stream = fileStream,
                    ContentType = avatar.ContentType
                };
            
                var command = new UploadAuctionAvatarCommand
                {
                    AuctionId = id,
                    Avatar = fileDto
                };

                var uri = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(uri);
            })
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}