using MediatR;
using Microsoft.Extensions.Logging;

namespace Auction.Application.Common.Mediator.PipelineBehaviours;

public class LoggingPipelineBehaviour<TRequest, TResponse>(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken = default
        )
    {
        logger.LogInformation(
            "Executing handler | request type - {RequestType} | response type - {ResponseType}", 
            typeof(TRequest),
            typeof(TResponse)
            );
        
        try
        {
           return await next.Invoke();
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                "Handler executed with exception | request type - {RequestType} | response type - {ResponseType} | exception - {Exception}", 
                typeof(TRequest),
                typeof(TResponse),
                exception
            );

            throw;
        }
    }
}
