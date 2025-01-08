using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SrsApi.Migrations
{
    /// <inheritdoc />
    public partial class addSrsItemsAndAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SrsFuzzySearchMethods",
                columns: table => new
                {
                    FuzzySearchMethod = table.Column<int>(type: "int", nullable: false),
                    FuzzySearchMethodName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsFuzzySearchMethods", x => x.FuzzySearchMethod);
                });

            migrationBuilder.CreateTable(
                name: "SrsItemLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsItemLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SrsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrsItems_SrsItemLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "SrsItemLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SrsAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SrsItemId = table.Column<int>(type: "int", nullable: true),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrsAnswers_SrsItems_SrsItemId",
                        column: x => x.SrsItemId,
                        principalTable: "SrsItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SrsAnswerFuzzySearchMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinumumAcceptedValue = table.Column<int>(type: "int", nullable: false),
                    SearchMethodFuzzySearchMethod = table.Column<int>(type: "int", nullable: false),
                    SrsAnswerId = table.Column<int>(type: "int", nullable: true),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsAnswerFuzzySearchMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrsAnswerFuzzySearchMethods_SrsAnswers_SrsAnswerId",
                        column: x => x.SrsAnswerId,
                        principalTable: "SrsAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SrsAnswerFuzzySearchMethods_SrsFuzzySearchMethods_SearchMethodFuzzySearchMethod",
                        column: x => x.SearchMethodFuzzySearchMethod,
                        principalTable: "SrsFuzzySearchMethods",
                        principalColumn: "FuzzySearchMethod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "44d79373-d381-42da-9e71-92e8e48abf60", null, "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "SrsFuzzySearchMethods",
                columns: new[] { "FuzzySearchMethod", "FuzzySearchMethodName" },
                values: new object[,]
                {
                    { 1, "ExactMatch" },
                    { 2, "SimpleRatio" },
                    { 3, "PartialRatio" },
                    { 4, "TokenSortRatio" },
                    { 5, "TokenSetRatio" },
                    { 6, "TokenInitialismRatio" },
                    { 7, "TokenAbbreviationRatio" },
                    { 8, "WeightedRatio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SrsAnswerFuzzySearchMethods_SearchMethodFuzzySearchMethod",
                table: "SrsAnswerFuzzySearchMethods",
                column: "SearchMethodFuzzySearchMethod");

            migrationBuilder.CreateIndex(
                name: "IX_SrsAnswerFuzzySearchMethods_SrsAnswerId",
                table: "SrsAnswerFuzzySearchMethods",
                column: "SrsAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SrsAnswers_SrsItemId",
                table: "SrsAnswers",
                column: "SrsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SrsItems_LevelId",
                table: "SrsItems",
                column: "LevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SrsAnswerFuzzySearchMethods");

            migrationBuilder.DropTable(
                name: "SrsAnswers");

            migrationBuilder.DropTable(
                name: "SrsFuzzySearchMethods");

            migrationBuilder.DropTable(
                name: "SrsItems");

            migrationBuilder.DropTable(
                name: "SrsItemLevels");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44d79373-d381-42da-9e71-92e8e48abf60");
        }
    }
}
