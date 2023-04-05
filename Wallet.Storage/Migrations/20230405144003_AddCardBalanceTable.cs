using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddCardBalanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardsBalance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Limit = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardsBalance_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardsBalance_UserId",
                table: "CardsBalance",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardsBalance");
        }
    }
}
