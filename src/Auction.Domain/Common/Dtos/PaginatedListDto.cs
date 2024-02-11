namespace Auction.Domain.Common.Dtos;

public record PaginatedListDto<TItem>
{
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public TItem[] Items { get; init; } = Array.Empty<TItem>();
}