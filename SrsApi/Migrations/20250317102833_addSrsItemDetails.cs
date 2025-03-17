using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SrsApi.Migrations
{
    /// <inheritdoc />
    public partial class addSrsItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetailsId",
                table: "SrsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SrsItemDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrsItemDetails", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 5,
                column: "FuzzySearchMethodName",
                value: "PartialTokenSortRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 6,
                column: "FuzzySearchMethodName",
                value: "TokenSetRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 7,
                column: "FuzzySearchMethodName",
                value: "PartialTokenSetRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 8,
                column: "FuzzySearchMethodName",
                value: "TokenInitialismRatio");

            migrationBuilder.InsertData(
                table: "SrsFuzzySearchMethods",
                columns: new[] { "FuzzySearchMethod", "FuzzySearchMethodName" },
                values: new object[,]
                {
                    { 9, "PartialTokenInitialismRatio" },
                    { 10, "TokenAbbreviationRatio" },
                    { 11, "PartialTokenAbbreviationRatio" },
                    { 12, "WeightedRatio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SrsItems_DetailsId",
                table: "SrsItems",
                column: "DetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems",
                column: "DetailsId",
                principalTable: "SrsItemDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems");

            migrationBuilder.DropTable(
                name: "SrsItemDetails");

            migrationBuilder.DropIndex(
                name: "IX_SrsItems_DetailsId",
                table: "SrsItems");

            migrationBuilder.DeleteData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "DetailsId",
                table: "SrsItems");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 5,
                column: "FuzzySearchMethodName",
                value: "TokenSetRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 6,
                column: "FuzzySearchMethodName",
                value: "TokenInitialismRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 7,
                column: "FuzzySearchMethodName",
                value: "TokenAbbreviationRatio");

            migrationBuilder.UpdateData(
                table: "SrsFuzzySearchMethods",
                keyColumn: "FuzzySearchMethod",
                keyValue: 8,
                column: "FuzzySearchMethodName",
                value: "WeightedRatio");
        }
    }
}
