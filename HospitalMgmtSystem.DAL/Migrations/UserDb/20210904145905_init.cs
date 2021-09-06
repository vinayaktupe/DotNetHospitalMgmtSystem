using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgmtSystem.DAL.Migrations.UserDb
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CasePapers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Doctor = table.Column<string>(nullable: true),
                    Patient = table.Column<string>(nullable: true),
                    PatientName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ForSelf = table.Column<bool>(nullable: false),
                    IsSolved = table.Column<bool>(nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValue: new DateTime(2021, 9, 4, 20, 29, 5, 507, DateTimeKind.Local).AddTicks(8456)),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePapers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(nullable: true),
                    Specialization = table.Column<int>(nullable: false),
                    YearsOfExperience = table.Column<double>(nullable: false),
                    AdditionalInfo = table.Column<string>(maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(nullable: true),
                    BloodGroup = table.Column<int>(nullable: false),
                    MedicalHistory = table.Column<string>(nullable: false),
                    AdditionalInfo = table.Column<string>(maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CaseFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePaperID = table.Column<int>(nullable: false),
                    FileType = table.Column<int>(nullable: false),
                    Fields = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValue: new DateTime(2021, 9, 4, 20, 29, 5, 508, DateTimeKind.Local).AddTicks(5032)),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CaseFiles_CasePapers_CasePaperID",
                        column: x => x.CasePaperID,
                        principalTable: "CasePapers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_CasePaperID",
                table: "CaseFiles",
                column: "CasePaperID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseFiles");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "CasePapers");
        }
    }
}
