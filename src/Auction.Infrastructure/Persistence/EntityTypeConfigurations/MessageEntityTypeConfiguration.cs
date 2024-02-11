using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.EntityTypeConfigurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder
            .HasIndex(message => message.CreatedAt)
            .HasMethod("btree");

        builder
            .HasOne(message => message.User)
            .WithMany(user => user.Messages)
            .HasForeignKey(message => message.UserId)
            .IsRequired();
    }
}