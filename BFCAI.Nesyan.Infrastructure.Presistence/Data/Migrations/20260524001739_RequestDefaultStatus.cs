using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequestDefaultStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RelativeDoctorRequests_PatientId",
                table: "RelativeDoctorRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RelativeDoctorRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_RelativeDoctorRequests_PatientId",
                table: "RelativeDoctorRequests",
                column: "PatientId",
                unique: true,
                filter: "[Status] = 'Accepted'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RelativeDoctorRequests_PatientId",
                table: "RelativeDoctorRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RelativeDoctorRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Pending");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeDoctorRequests_PatientId",
                table: "RelativeDoctorRequests",
                column: "PatientId",
                unique: true,
                filter: "[Status] = 'Selected'");
        }
    }
}
