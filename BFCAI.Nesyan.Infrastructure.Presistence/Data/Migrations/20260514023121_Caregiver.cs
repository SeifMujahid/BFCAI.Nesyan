using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Caregiver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaregiverId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CaregiverId",
                table: "Patients",
                column: "CaregiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Caregivers_CaregiverId",
                table: "Patients",
                column: "CaregiverId",
                principalTable: "Caregivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Caregivers_CaregiverId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CaregiverId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CaregiverId",
                table: "Patients");
        }
    }
}
