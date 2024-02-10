using Auction.Domain.Common;
using MediatR;

namespace Auction.Application.Common.Mediator.PipelineBehaviours;

public class TransactionalPipelineBehaviour<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken = default
        )
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await next.Invoke();

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
