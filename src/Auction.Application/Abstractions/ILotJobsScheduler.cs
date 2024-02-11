namespace Auction.Application.Abstractions;

public interface ILotJobsScheduler
{
    Task ScheduleOpenLotAsync(
        Guid lotId, 
        DateTimeOffset timestamp
        );
    
    Task ScheduleCloseLotAsync(
        Guid lotId, 
        DateTimeOffset timestamp
    );
}