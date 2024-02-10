using Auction.Application.Dtos;
using Auction.Domain.Entities;
using Mapster;

namespace Auction.Application.Mappers;

public static class AuctionMapper
{
    public static void RegisterAuctionConfiguration()
    {
        TypeAdapterConfig<AuctionEntity, AuctionDto>.NewConfig();
    }
}