using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrsApi.Migrations
{
    /// <inheritdoc />
    public partial class fixMinimumAcceptedValueTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinumumAcceptedValue",
                table: "SrsAnswerFuzzySearchMethods",
                newName: "MinimumAcceptedValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinimumAcceptedValue",
                table: "SrsAnswerFuzzySearchMethods",
                newName: "MinumumAcceptedValue");
        }
    }
}
