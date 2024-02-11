using Auction.Application.Abstractions;
using Auction.Infrastructure.Implementations.Jobs;
using Quartz;

namespace Auction.Infrastructure.Implementations;

public class LotJobsScheduler(ISchedulerFactory schedulerFactory) : ILotJobsScheduler
{
    public async Task ScheduleOpenLotAsync(
        Guid lotId,
        DateTimeOffset timestamp
        )
    {
        var jobDetail = JobBuilder
            .Create<OpenLotJob>()
            .SetJobData(new JobDataMap
            {
                ["lotId"] = lotId.ToString(),
            })
            .Build();

        var trigger = TriggerBuilder
            .Create()
            .StartAt(timestamp)
            .Build();

        var scheduler = await schedulerFactory.GetScheduler();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }

    public async Task ScheduleCloseLotAsync(
        Guid lotId, 
        DateTimeOffset timestamp
        )
    {
        var jobDetail = JobBuilder
            .Create<CloseLotJob>()
            .SetJobData(new JobDataMap
            {
                ["lotId"] = lotId.ToString(),
            })
            .Build();

        var trigger = TriggerBuilder
            .Create()
            .StartAt(timestamp)
            .Build();

        var scheduler = await schedulerFactory.GetScheduler();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}