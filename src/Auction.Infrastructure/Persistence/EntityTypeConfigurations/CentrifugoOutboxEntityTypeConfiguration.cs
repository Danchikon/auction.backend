using System.Text.Json.Nodes;
using Auction.Domain.Entities;
using Auction.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.EntityTypeConfigurations;

public class CentrifugoOutboxEntityTypeConfiguration : IEntityTypeConfiguration<CentrifugoOutboxEntity>
{
    public void Configure(EntityTypeBuilder<CentrifugoOutboxEntity> builder)
    {
        builder.ToTable("centrifugo_outbox");
            
        builder
            .HasKey(outbox => outbox.Id);
        
        builder
            .Property(outbox => outbox.Method)
            .HasColumnType("text")
            .HasColumnName("method")
            .IsRequired();
        
        builder
            .Property(outbox => outbox.Payload)
            .HasConversion(
                convertToProviderExpression: jsonObject => jsonObject.ToJsonString(null),
                convertFromProviderExpression: @string => JsonNode.Parse(@string, null, default)!.AsObject()
                )
            .HasColumnType("jsonb")
            .HasColumnName("payload")
            .IsRequired();
        
        builder
            .Property(outbox => outbox.Partition)
            .HasColumnType("integer")
            .HasColumnName("partition")
            .HasDefaultValueSql("0")
            .IsRequired();
        
        builder
            .Property(outbox => outbox.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at")
            .IsRequired();
    }
}