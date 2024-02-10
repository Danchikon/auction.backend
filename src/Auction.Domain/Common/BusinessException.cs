namespace Auction.Domain.Common;

public abstract class BusinessException(string message) : Exception(message);