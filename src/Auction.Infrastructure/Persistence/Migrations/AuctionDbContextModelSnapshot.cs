﻿// <auto-generated />
using System;
using System.Text.Json.Nodes;
using Auction.Domain.Enums;
using Auction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auction.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    partial class AuctionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "auction_state", new[] { "scheduled", "opened", "closed" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "lot_state", new[] { "waiting", "in_sale", "sold" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Auction.Domain.Entities.AuctionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("ClosedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("closed_at");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset?>("OpenedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("opened_at");

                    b.Property<DateTimeOffset?>("OpensAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("opens_at");

                    b.Property<AuctionState>("State")
                        .HasColumnType("auction_state")
                        .HasColumnName("state");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_auctions");

                    b.HasIndex("State")
                        .HasDatabaseName("ix_auctions_state");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("State"), "hash");

                    b.HasIndex("Title")
                        .HasDatabaseName("ix_auctions_title");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Title"), "btree");

                    b.ToTable("auctions", (string)null);
                });

            modelBuilder.Entity("Auction.Domain.Entities.BidEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bids");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_bids_user_id");

                    b.ToTable("bids", (string)null);
                });

            modelBuilder.Entity("Auction.Domain.Entities.LotEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AuctionEntityId")
                        .HasColumnType("uuid")
                        .HasColumnName("auction_entity_id");

                    b.Property<Guid?>("BestBidId")
                        .HasColumnType("uuid")
                        .HasColumnName("best_bid_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<decimal>("MinPriceStepSize")
                        .HasColumnType("numeric")
                        .HasColumnName("min_price_step_size");

                    b.Property<decimal>("StartPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("start_price");

                    b.Property<LotState>("State")
                        .HasColumnType("lot_state")
                        .HasColumnName("state");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_lots");

                    b.HasIndex("AuctionEntityId")
                        .HasDatabaseName("ix_lots_auction_entity_id");

                    b.HasIndex("BestBidId")
                        .HasDatabaseName("ix_lots_best_bid_id");

                    b.HasIndex("State")
                        .HasDatabaseName("ix_lots_state");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("State"), "hash");

                    b.HasIndex("Title")
                        .HasDatabaseName("ix_lots_title");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Title"), "btree");

                    b.ToTable("lots", (string)null);
                });

            modelBuilder.Entity("Auction.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("text")
                        .HasColumnName("avatar");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Auction.Infrastructure.Entities.CentrifugoOutboxEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("method");

                    b.Property<int>("Partition")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("partition")
                        .HasDefaultValueSql("0");

                    b.Property<JsonObject>("Payload")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("payload");

                    b.HasKey("Id")
                        .HasName("pk_centrifugo_outbox");

                    b.ToTable("centrifugo_outbox", (string)null);
                });

            modelBuilder.Entity("Auction.Domain.Entities.BidEntity", b =>
                {
                    b.HasOne("Auction.Domain.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bids_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Auction.Domain.Entities.LotEntity", b =>
                {
                    b.HasOne("Auction.Domain.Entities.AuctionEntity", null)
                        .WithMany("Lots")
                        .HasForeignKey("AuctionEntityId")
                        .HasConstraintName("fk_lots_auctions_auction_entity_id");

                    b.HasOne("Auction.Domain.Entities.BidEntity", "BestBid")
                        .WithMany()
                        .HasForeignKey("BestBidId")
                        .HasConstraintName("fk_lots_bids_best_bid_id");

                    b.Navigation("BestBid");
                });

            modelBuilder.Entity("Auction.Domain.Entities.AuctionEntity", b =>
                {
                    b.Navigation("Lots");
                });
#pragma warning restore 612, 618
        }
    }
}
