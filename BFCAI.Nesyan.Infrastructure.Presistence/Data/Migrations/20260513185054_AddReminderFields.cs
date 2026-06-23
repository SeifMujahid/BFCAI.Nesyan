using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Reminders");
        }
    }
}
