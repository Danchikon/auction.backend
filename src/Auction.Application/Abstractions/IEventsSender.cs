using Auction.Application.Dtos;

namespace Auction.Application.Abstractions;

public interface IEventsSender
{
    Task PublishAsync<TData>(EventDto<TData> @event, CancellationToken cancellationToken = default);
}