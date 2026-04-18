using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Relatives");

            migrationBuilder.DropColumn(
                name: "ConfusionFrequency",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "LanguageProblems",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "MemoryLossFrequency",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "MoodChanges",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "RepetitiveBehavior",
                table: "RelativeDoctorRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MindGameSessions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MindGameSessions");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "MindGameSessions");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "MindGameSessions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MindGames");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MindGames");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MindGames");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "MindGames");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "MindGames");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "AssignedOn",
                table: "MindGameSessions",
                newName: "AddedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "MindGames",
                newName: "Brief");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                table: "PatientRelativeAlerts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PatientRelativeAlerts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScheduleInstructions = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medications_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PatientId",
                table: "Medications",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.RenameColumn(
                name: "AddedDate",
                table: "MindGameSessions",
                newName: "AssignedOn");

            migrationBuilder.RenameColumn(
                name: "Brief",
                table: "MindGames",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Relatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConfusionFrequency",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RelativeDoctorRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LanguageProblems",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "RelativeDoctorRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MemoryLossFrequency",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MoodChanges",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RepetitiveBehavior",
                table: "RelativeDoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                table: "PatientRelativeAlerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PatientRelativeAlerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MindGameSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MindGameSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "MindGameSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "MindGameSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MindGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MindGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MindGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "MindGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "MindGames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
