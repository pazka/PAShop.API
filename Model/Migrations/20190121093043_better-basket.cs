using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class betterbasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.AddColumn<Guid>(
                name: "BasketId",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BasketId",
                table: "Items",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Baskets_BasketId",
                table: "Items",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Baskets_BasketId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BasketId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    BasketId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => new { x.BasketId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BasketItem_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_ItemId",
                table: "BasketItem",
                column: "ItemId");
        }
    }
}
