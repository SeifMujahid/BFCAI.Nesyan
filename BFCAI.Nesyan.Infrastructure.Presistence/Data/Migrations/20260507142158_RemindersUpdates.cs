using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemindersUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Doctors_DoctorId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Patients_PatientId",
                table: "Medications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medications",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "ScheduleInstructions",
                table: "Medications");

            migrationBuilder.RenameTable(
                name: "Medications",
                newName: "Reminders");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Reminders",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Medications_PatientId",
                table: "Reminders",
                newName: "IX_Reminders_PatientId");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Reminders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Reminders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "Reminders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReminderDate",
                table: "Reminders",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ReminderTime",
                table: "Reminders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reminders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reminders",
                table: "Reminders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Patients_PatientId",
                table: "Reminders",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Patients_PatientId",
                table: "Reminders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reminders",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "ReminderDate",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reminders");

            migrationBuilder.RenameTable(
                name: "Reminders",
                newName: "Medications");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Medications",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_PatientId",
                table: "Medications",
                newName: "IX_Medications_PatientId");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Medications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Frequency",
                table: "Medications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "Medications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScheduleInstructions",
                table: "Medications",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medications",
                table: "Medications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Doctors_DoctorId",
                table: "Medications",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Patients_PatientId",
                table: "Medications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
