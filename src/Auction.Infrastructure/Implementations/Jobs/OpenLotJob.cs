using Auction.Application.Abstractions;
using Auction.Application.Mediator.Commands.Lots;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Auction.Infrastructure.Implementations.Jobs;

public class OpenLotJob(
    [FromKeyedServices("lot_actions")] SemaphoreSlim semaphore,
    IMediator mediator
    ) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var lotIdString = context.MergedJobDataMap.GetString("lotId")!;

        var command = new OpenLotCommand
        {
            LotId = Guid.Parse(lotIdString)
        };

        await semaphore.WaitAsync();
        
        try
        {
            await mediator.Send(command, context.CancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }
}