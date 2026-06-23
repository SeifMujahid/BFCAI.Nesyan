using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PatientRelatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PatientRelatives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "PatientRelatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "PatientRelatives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PatientRelatives");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PatientRelatives");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PatientRelatives");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "PatientRelatives");
        }
    }
}
