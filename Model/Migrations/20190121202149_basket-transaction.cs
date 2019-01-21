﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class baskettransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Transactions_Id",
                table: "Baskets");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Baskets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_TransactionId",
                table: "Baskets",
                column: "TransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Transactions_TransactionId",
                table: "Baskets",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Transactions_TransactionId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_TransactionId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Baskets");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Transactions_Id",
                table: "Baskets",
                column: "Id",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
