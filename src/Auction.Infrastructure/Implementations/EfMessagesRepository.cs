using Auction.Domain.Entities;
using Auction.Infrastructure.Persistence;

namespace Auction.Infrastructure.Implementations;

public class EfMessagesRepository(AuctionDbContext dbContext) : EfRepository<MessageEntity, AuctionDbContext>(dbContext)
{
    private readonly AuctionDbContext _dbContext = dbContext;

    public override async Task<MessageEntity> AddAsync(MessageEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext
            .Messages
            .AddAsync(entity, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await entry
            .Reference(message => message.User)
            .LoadAsync(cancellationToken);
        
        return entry.Entity;
    }
}