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
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "Share" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8294), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8295), "AAPL", 150m },
                    { 2, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8308), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8310), "META", 186m },
                    { 3, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8317), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8319), "MSFT", 109m },
                    { 4, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8328), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8329), "GOOGL", 199m },
                    { 5, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8336), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8338), "AMZN", 155m }
                });

            migrationBuilder.InsertData(
                table: "Tactics",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8169), null, "AKO JE TRENUTNA CENA VECA OD PROSECNE CENE POSLEDNJIH PET INTERVALA", "First" },
                    { 2, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8248), null, "SREDSTVA KOJA KORISNIK IMA * KOEFICIJENT = TO JE CIFRA KOJOM KUPUJE", "Second" },
                    { 3, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8261), null, null, "Third" },
                    { 4, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8270), null, null, "Fourth" },
                    { 5, new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8278), null, null, "Fifth" }
                });

            migrationBuilder.InsertData(
                table: "BuyingSelingShares",
                columns: new[] { "Id", "CompanyId", "DateCreated", "DateUpdated", "Price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 8, 24, 23, 31, 59, 334, DateTimeKind.Local).AddTicks(8353), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8357), 102m },
                    { 2, 1, new DateTime(2023, 8, 24, 23, 32, 59, 334, DateTimeKind.Local).AddTicks(8368), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8370), 107m },
                    { 3, 1, new DateTime(2023, 8, 24, 23, 37, 59, 334, DateTimeKind.Local).AddTicks(8378), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8380), 103.5m },
                    { 4, 1, new DateTime(2023, 8, 24, 23, 37, 59, 334, DateTimeKind.Local).AddTicks(8387), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8389), 102m },
                    { 5, 1, new DateTime(2023, 8, 24, 23, 37, 59, 334, DateTimeKind.Local).AddTicks(8397), new DateTime(2023, 8, 24, 23, 29, 59, 334, DateTimeKind.Local).AddTicks(8398), 104m }
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
