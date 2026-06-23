using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssesmentsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecognitionOfName = table.Column<int>(type: "int", nullable: false),
                    RecognitionOfPlace = table.Column<int>(type: "int", nullable: false),
                    RecognitionOfTime = table.Column<int>(type: "int", nullable: false),
                    AbilityToConcentrate = table.Column<int>(type: "int", nullable: false),
                    RecallOfRecentEvents = table.Column<int>(type: "int", nullable: false),
                    AnxietyOrStress = table.Column<int>(type: "int", nullable: false),
                    DepressionOrSadness = table.Column<int>(type: "int", nullable: false),
                    Aggression = table.Column<int>(type: "int", nullable: false),
                    EatingAndDrinking = table.Column<int>(type: "int", nullable: false),
                    Bathing = table.Column<int>(type: "int", nullable: false),
                    Dressing = table.Column<int>(type: "int", nullable: false),
                    UsingBathroom = table.Column<int>(type: "int", nullable: false),
                    Mobility = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_PatientId",
                table: "Assessments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");
        }
    }
}
