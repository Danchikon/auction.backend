using System.Reflection;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence;

public class AuctionDbContext : DbContext
{
    public required DbSet<UserEntity> Users { get; init; }
    public required DbSet<AuctionEntity> Auctions { get; init; }
    public required DbSet<LotEntity> Lots { get; init; }
    public required DbSet<BidEntity> Bids { get; init; }
    
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<AuctionState>();
        modelBuilder.HasPostgresEnum<LotState>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}