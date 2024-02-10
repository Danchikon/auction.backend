using MediatR;

namespace Auction.Application.Common.Mediator;

public abstract record CommandBase : IRequest;

public abstract record CommandBase<TResponse> : IRequest<TResponse>;