using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrsApi.Migrations
{
    /// <inheritdoc />
    public partial class setSrsItemDetailsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems");

            migrationBuilder.AlterColumn<int>(
                name: "DetailsId",
                table: "SrsItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems",
                column: "DetailsId",
                principalTable: "SrsItemDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems");

            migrationBuilder.AlterColumn<int>(
                name: "DetailsId",
                table: "SrsItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SrsItems_SrsItemDetails_DetailsId",
                table: "SrsItems",
                column: "DetailsId",
                principalTable: "SrsItemDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
