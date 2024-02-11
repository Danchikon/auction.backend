using System.Security.Claims;
using Auction.Api.Dtos;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Application.Mediator.Queries.Auctions;
using Auction.Domain.Common;
using Auction.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class AuctionsRouter
{
    public static void MapAuctionsRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/",async (
            [FromBody] CreateAuctionDto dto,
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

            var command = new CreateAuctionCommand
            {
                UserId = Guid.Parse(userIdString),
                Description = dto.Description,
                Lots = dto.Lots,
                Title = dto.Title,
            };
            
            var auctionDto = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(auctionDto); 
        }).RequireAuthorization();

        endpoints.MapGet("/{id:guid}", async (
            [FromRoute] Guid id,
            IMediator mediator,
            CancellationToken cancellationToken
        ) =>
        {
            var query = new GetAuctionQuery
            {
                Id = id
            };

            var auctionDto = await mediator.Send(query, cancellationToken);

            return Results.Ok(auctionDto);
        });

        endpoints.MapGet("/", async (
            [FromQuery] AuctionState? state,
            [FromQuery] string? title,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            IMediator mediator,
            CancellationToken cancellationToken
        ) =>
        {
            var query = new GetAuctionsQuery
            {
                State = state,
                Title = title,
                Pagination = new PaginationOptions
                {
                    Page = page ?? PaginationOptions.DefaultPage,
                    PageSize = pageSize ?? PaginationOptions.DefaultPageSize
                }
            };

            var auctionsDtos = await mediator.Send(query, cancellationToken);

            return Results.Ok(auctionsDtos);
        });
        
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
                {
                    return Results.Forbid();
                }
                
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