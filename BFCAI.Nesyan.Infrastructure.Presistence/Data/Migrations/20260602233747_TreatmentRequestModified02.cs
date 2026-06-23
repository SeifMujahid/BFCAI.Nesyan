using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class TreatmentRequestModified02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TreatmentRequest_PatientId",
                table: "TreatmentRequest");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRequest_PatientId_CaregiverId",
                table: "TreatmentRequest",
                columns: new[] { "PatientId", "CaregiverId" },
                unique: true,
                filter: "[Status] = 'Selected'");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRequest_PatientId_DoctorId",
                table: "TreatmentRequest",
                columns: new[] { "PatientId", "DoctorId" },
                unique: true,
                filter: "[Status] = 'Selected'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TreatmentRequest_PatientId_CaregiverId",
                table: "TreatmentRequest");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentRequest_PatientId_DoctorId",
                table: "TreatmentRequest");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRequest_PatientId",
                table: "TreatmentRequest",
                column: "PatientId",
                unique: true,
                filter: "[Status] = 'Selected'");
        }
    }
}
