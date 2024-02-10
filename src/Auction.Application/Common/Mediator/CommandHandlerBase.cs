using MediatR;

namespace Auction.Application.Common.Mediator;

public abstract class CommandHandlerBase<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand: CommandBase<TResponse>
{
    public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand> where TCommand: CommandBase
{
    public abstract Task Handle(TCommand command, CancellationToken cancellationToken = default);
}