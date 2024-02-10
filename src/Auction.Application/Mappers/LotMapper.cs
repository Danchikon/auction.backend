using Auction.Application.Dtos;
using Auction.Domain.Entities;
using Mapster;

namespace Auction.Application.Mappers;

public class LotMapper
{
    public static void RegisterLotConfiguration()
    {
        TypeAdapterConfig<LotEntity, LotDto>.NewConfig();
    }
}