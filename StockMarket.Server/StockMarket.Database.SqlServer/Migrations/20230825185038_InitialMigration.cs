using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockMarket.Database.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Share = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tactics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tactics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceFluctuations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceFluctuations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceFluctuations_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BalanceFunds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RiskCoefficient = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TacticsId = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tactics_TacticsId",
                        column: x => x.TacticsId,
                        principalTable: "Tactics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Shares = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUsers", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyUsers_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Companys",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "Share" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7066), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7068), "AAPL", 150m },
                    { 2, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7110), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7111), "META", 186m },
                    { 3, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7121), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7122), "MSFT", 109m },
                    { 4, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7131), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7132), "GOOGL", 199m },
                    { 5, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7141), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7143), "AMZN", 155m }
                });

            migrationBuilder.InsertData(
                table: "Tactics",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(6941), null, "IF THE CURRENT PRICE IS HIGHER THAN THE AVERAGE PRICE OF THE LAST FIVE INTERVALS", "First" },
                    { 2, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7024), null, "FUNDS THAT THE USER HAS * COEFFICIENT = IT IS THE FIGURE WITH WHICH HE BUYS", "Second" },
                    { 3, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7034), null, null, "Third" },
                    { 4, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7042), null, null, "Fourth" },
                    { 5, new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7051), null, null, "Fifth" }
                });

            migrationBuilder.InsertData(
                table: "PriceFluctuations",
                columns: new[] { "Id", "CompanyId", "DateCreated", "DateUpdated", "Price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 8, 25, 20, 52, 38, 188, DateTimeKind.Local).AddTicks(7166), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7171), 102m },
                    { 2, 1, new DateTime(2023, 8, 25, 20, 53, 38, 188, DateTimeKind.Local).AddTicks(7181), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7184), 107m },
                    { 3, 1, new DateTime(2023, 8, 25, 20, 58, 38, 188, DateTimeKind.Local).AddTicks(7193), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7195), 103.5m },
                    { 4, 1, new DateTime(2023, 8, 25, 20, 58, 38, 188, DateTimeKind.Local).AddTicks(7202), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7204), 102m },
                    { 5, 1, new DateTime(2023, 8, 25, 20, 58, 38, 188, DateTimeKind.Local).AddTicks(7212), new DateTime(2023, 8, 25, 20, 50, 38, 188, DateTimeKind.Local).AddTicks(7213), 104m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId",
                table: "CompanyUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceFluctuations_CompanyId",
                table: "PriceFluctuations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TacticsId",
                table: "Users",
                column: "TacticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUsers");

            migrationBuilder.DropTable(
                name: "PriceFluctuations");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tactics");
        }
    }
}
