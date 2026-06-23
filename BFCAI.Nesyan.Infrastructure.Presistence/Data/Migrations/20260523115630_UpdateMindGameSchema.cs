using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMindGameSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetMetrics",
                table: "MindGames",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MindGames",
                newName: "Subtitle");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "MindGames",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "Brief",
                table: "MindGames",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MindGames",
                newName: "TargetMetrics");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "MindGames",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "MindGames",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "MindGames",
                newName: "Brief");
        }
    }
}
