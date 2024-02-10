using Auction.Domain.Common;

namespace Auction.Domain.Exceptions;

public class NotFoundException : BusinessException
{
    private NotFoundException(string message) : base(message)
    {
        
    }
    public static NotFoundException For<TEntity>()
    {
        return new NotFoundException($"Entity {nameof(TEntity)} not found");
    }
}