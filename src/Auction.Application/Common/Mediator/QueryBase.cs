using MediatR;

namespace Auction.Application.Common.Mediator;

public abstract record QueryBase<TResponse> : IRequest<TResponse>;
