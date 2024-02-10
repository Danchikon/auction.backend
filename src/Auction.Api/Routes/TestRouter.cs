using System.Text.Json.Nodes;
using Auction.Application.Abstractions;
using Auction.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Api.Routes;

public static class TestRouter
{
    public static void MapTestRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/events", async (
            [FromBody] object payload,
            IEventsPublisher eventsPublisher,
            CancellationToken cancellationToken
        ) =>
        {
            var dto = new EventDto<object>
            {
                Channel = "test-channel",
                Data = payload
            };

            await eventsPublisher.PublishAsync(dto, cancellationToken);
            
            return Results.NoContent();
        });
    }
}