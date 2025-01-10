using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrsApi.Migrations
{
    /// <inheritdoc />
    public partial class addSrsItemOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SrsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "SrsItems");
        }
    }
}
