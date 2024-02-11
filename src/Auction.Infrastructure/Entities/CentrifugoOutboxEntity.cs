using System.Text.Json;
using System.Text.Json.Nodes;

namespace Auction.Infrastructure.Entities;

public class CentrifugoOutboxEntity 
{
    public required long Id { get; init; }
    public required string Method { get; set; } 
    public required string Payload { get; set; } 
    public required int Partition { get; set; } 
    public required DateTimeOffset CreatedAt { get; init; }
}