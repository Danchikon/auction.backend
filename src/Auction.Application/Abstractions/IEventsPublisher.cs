using Auction.Application.Dtos;

namespace Auction.Application.Abstractions;

public interface IEventsPublisher
{
    Task PublishAsync<TData>(EventDto<TData> @event, CancellationToken cancellationToken = default);
}