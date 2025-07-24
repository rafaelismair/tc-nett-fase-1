using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.CloudGames.Fase1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionWithGameRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionTitle = table.Column<string>(type: "text", nullable: false),
                    PromotionDescription = table.Column<string>(type: "text", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePromotions",
                columns: table => new
                {
                    GamePromotionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePromotions", x => x.GamePromotionId);
                    table.ForeignKey(
                        name: "FK_GamePromotions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$KB4AoC8oR1WsGZQHRUwCVeYxTHykOwnkD2Dk.g9sEPT0VsUo4jksi");

            migrationBuilder.CreateIndex(
                name: "IX_GamePromotions_GameId",
                table: "GamePromotions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePromotions_PromotionId",
                table: "GamePromotions",
                column: "PromotionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePromotions");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$R3AikLMFV4fvnSydHIDlBOrYa0rBCdeH/DoZr4q8QxOdbZLCJ7jxK");
        }
    }
}
