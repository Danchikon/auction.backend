using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.EntityTypeConfigurations;

public class LotEntityTypeConfiguration : IEntityTypeConfiguration<LotEntity>
{
    public void Configure(EntityTypeBuilder<LotEntity> builder)
    {
        builder
            .Property(lot => lot.State)
            .HasColumnType("lon_state");
        
        builder
            .HasIndex(lot => lot.State)
            .HasMethod("hash");
        
        builder
            .HasIndex(lot => lot.Title)
            .HasMethod("btree");
    }
}