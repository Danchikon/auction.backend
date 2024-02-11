using Auction.Domain.Common;
using Auction.Domain.Common.Errors;

namespace Auction.Domain.Exceptions;

public class AlreadyExistsException : BusinessException
{
    private  AlreadyExistsException(string message) : base(message)
    {
        
    }
    public static AlreadyExistsException For<TEntity>()
    {
        return new AlreadyExistsException($"Entity {nameof(TEntity)} already exists")
        {
            ErrorKind = ErrorKind.InvalidOperation
        };
    }
}