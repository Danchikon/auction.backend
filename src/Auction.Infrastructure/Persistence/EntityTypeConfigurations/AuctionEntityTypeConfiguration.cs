using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.EntityTypeConfigurations;

public class AuctionEntityTypeConfiguration : IEntityTypeConfiguration<AuctionEntity>
{
    public void Configure(EntityTypeBuilder<AuctionEntity> builder)
    {
        builder
            .Property(auction => auction.State)
            .HasColumnType("auction_state");
        
        builder
            .HasIndex(auction => auction.State)
            .HasMethod("hash");
        
        builder
            .HasIndex(auction => auction.Title)
            .HasMethod("btree");
    }
}