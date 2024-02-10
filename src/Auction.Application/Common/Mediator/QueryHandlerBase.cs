using MediatR;

namespace Auction.Application.Common.Mediator;

public abstract class QueryHandlerBase<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery: QueryBase<TResponse>
{
    public abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}