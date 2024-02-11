using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Auction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lots_auctions_auction_entity_id",
                table: "lots");

            migrationBuilder.DropForeignKey(
                name: "fk_lots_bids_best_bid_id",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "ix_lots_auction_entity_id",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "ix_lots_best_bid_id",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "auction_entity_id",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "best_bid_id",
                table: "lots");

            migrationBuilder.AddColumn<Guid>(
                name: "auction_id",
                table: "lots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "lots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "lot_entity_id",
                table: "bids",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "auctions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "auctions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_lots_auction_id",
                table: "lots",
                column: "auction_id");

            migrationBuilder.CreateIndex(
                name: "ix_bids_lot_entity_id",
                table: "bids",
                column: "lot_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_auctions_user_id",
                table: "auctions",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_auctions_users_user_id",
                table: "auctions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bids_lots_lot_entity_id",
                table: "bids",
                column: "lot_entity_id",
                principalTable: "lots",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lots_auctions_auction_id",
                table: "lots",
                column: "auction_id",
                principalTable: "auctions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_auctions_users_user_id",
                table: "auctions");

            migrationBuilder.DropForeignKey(
                name: "fk_bids_lots_lot_entity_id",
                table: "bids");

            migrationBuilder.DropForeignKey(
                name: "fk_lots_auctions_auction_id",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "ix_lots_auction_id",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "ix_bids_lot_entity_id",
                table: "bids");

            migrationBuilder.DropIndex(
                name: "ix_auctions_user_id",
                table: "auctions");

            migrationBuilder.DropColumn(
                name: "auction_id",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "lot_entity_id",
                table: "bids");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "auctions");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "auctions");

            migrationBuilder.AddColumn<Guid>(
                name: "auction_entity_id",
                table: "lots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "best_bid_id",
                table: "lots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_lots_auction_entity_id",
                table: "lots",
                column: "auction_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_lots_best_bid_id",
                table: "lots",
                column: "best_bid_id");

            migrationBuilder.AddForeignKey(
                name: "fk_lots_auctions_auction_entity_id",
                table: "lots",
                column: "auction_entity_id",
                principalTable: "auctions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lots_bids_best_bid_id",
                table: "lots",
                column: "best_bid_id",
                principalTable: "bids",
                principalColumn: "id");
        }
    }
}
