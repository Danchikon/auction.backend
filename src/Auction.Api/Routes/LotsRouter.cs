using System.Security.Claims;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Lots;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class LotsRouter
{
    public static void MapLotsRoutes(this IEndpointRouteBuilder endpoints)
    {
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
            
                var command = new UploadLotAvatarCommand
                {
                    LotId = id,
                    Avatar = fileDto
                };

                var uri = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(uri);
            })
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}