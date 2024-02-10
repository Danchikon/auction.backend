using Auction.Application.Mappers;

namespace Auction.Api.Configurations;

public static class Mapping
{
    public static void RegisterMappers()
    {
        AuctionMapper.RegisterAuctionConfiguration();
        LotMapper.RegisterLotConfiguration();
    }
}