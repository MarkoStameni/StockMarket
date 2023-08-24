using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockMarket.Database.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialValue : Migration
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
                    OpenPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HighPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LowPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Share = table.Column<int>(type: "int", nullable: false),
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
                name: "BuyingSelingShares",
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
                    table.PrimaryKey("PK_BuyingSelingShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingSelingShares_Companys_CompanyId",
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
                columns: new[] { "Id", "ClosePrice", "DateCreated", "DateUpdated", "HighPrice", "LowPrice", "Name", "OpenPrice", "Share" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(3977), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4044), null, null, "AAPL", 100m, 150 },
                    { 2, null, new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4071), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4073), null, null, "META", 234m, 186 },
                    { 3, null, new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4084), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4085), null, null, "MSFT", 88m, 109 },
                    { 4, null, new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4136), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4139), null, null, "GOOGL", 321m, 199 },
                    { 5, null, new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4151), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4152), null, null, "AMZN", 118m, 155 }
                });

            migrationBuilder.InsertData(
                table: "BuyingSelingShares",
                columns: new[] { "Id", "CompanyId", "DateCreated", "DateUpdated", "Price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 8, 24, 20, 8, 33, 113, DateTimeKind.Local).AddTicks(4168), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4178), 102m },
                    { 2, 1, new DateTime(2023, 8, 24, 20, 9, 33, 113, DateTimeKind.Local).AddTicks(4189), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4190), 107m },
                    { 3, 1, new DateTime(2023, 8, 24, 20, 14, 33, 113, DateTimeKind.Local).AddTicks(4202), new DateTime(2023, 8, 24, 20, 6, 33, 113, DateTimeKind.Local).AddTicks(4204), 103.5m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyingSelingShares_CompanyId",
                table: "BuyingSelingShares",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId",
                table: "CompanyUsers",
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
                name: "BuyingSelingShares");

            migrationBuilder.DropTable(
                name: "CompanyUsers");

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
