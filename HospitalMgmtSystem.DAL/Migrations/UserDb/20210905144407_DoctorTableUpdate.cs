using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgmtSystem.DAL.Migrations.UserDb
{
    public partial class DoctorTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 5, 20, 14, 7, 89, DateTimeKind.Local).AddTicks(6996),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 5, 20, 11, 22, 588, DateTimeKind.Local).AddTicks(4102));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 5, 20, 14, 7, 90, DateTimeKind.Local).AddTicks(4115),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 5, 20, 11, 22, 588, DateTimeKind.Local).AddTicks(9038));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 5, 20, 11, 22, 588, DateTimeKind.Local).AddTicks(4102),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 5, 20, 14, 7, 89, DateTimeKind.Local).AddTicks(6996));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 5, 20, 11, 22, 588, DateTimeKind.Local).AddTicks(9038),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 5, 20, 14, 7, 90, DateTimeKind.Local).AddTicks(4115));
        }
    }
}
