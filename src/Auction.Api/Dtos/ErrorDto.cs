using Auction.Domain.Common.Errors;

namespace Auction.Api.Dtos;

public record ErrorDto
{
    public required ErrorKind Kind { get; init; }
    public ICollection<string> Messages { get; init; } = new List<string>();
};