using Auction.Domain.Common.Errors;

namespace Auction.Domain.Common;

public abstract class BusinessException(string message) : Exception(message)
{
    public ErrorKind ErrorKind { get; init; }
} 