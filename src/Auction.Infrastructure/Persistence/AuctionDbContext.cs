using System.Reflection;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Auction.Infrastructure.Persistence;

public class AuctionDbContext : DbContext
{
    public required DbSet<UserEntity> Users { get; init; } 
    public required DbSet<AuctionEntity> Auctions { get; init; }
    public required DbSet<LotEntity> Lots { get; init; }
    public required DbSet<BidEntity> Bids { get; init; }
    public required DbSet<MessageEntity> Messages { get; init; }
    public required DbSet<CentrifugoOutboxEntity> CentrifugoOutboxs { get; init; }
    
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<AuctionState>();
        modelBuilder.HasPostgresEnum<LotState>();
        
        modelBuilder
            .Entity<CentrifugoOutboxEntity>()
            .ToTable("centrifugo_outbox", tableBuilder => tableBuilder.ExcludeFromMigrations())
            .Property(outbox => outbox.Payload)
            .HasColumnType("jsonb");
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}