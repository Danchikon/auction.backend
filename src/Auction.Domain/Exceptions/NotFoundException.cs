using Auction.Domain.Common;
using Auction.Domain.Common.Errors;

namespace Auction.Domain.Exceptions;

public class NotFoundException : BusinessException
{
    private NotFoundException(string message) : base(message)
    {
        
    }
    public static NotFoundException For<TEntity>()
    {
        return new NotFoundException($"Entity {nameof(TEntity)} not found")
        {
            ErrorKind = ErrorKind.NotFound
        };
    }
}