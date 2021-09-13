using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgmtSystem.DAL.Migrations
{
    public partial class CaseFileTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(6631),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 8, 13, 45, 21, 783, DateTimeKind.Local).AddTicks(7919));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(7791),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CaseFiles",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 888, DateTimeKind.Local).AddTicks(6960),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 8, 13, 45, 21, 779, DateTimeKind.Local).AddTicks(338));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CaseFiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 8, 13, 45, 21, 783, DateTimeKind.Local).AddTicks(7919),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(6631));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(7791));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 8, 13, 45, 21, 779, DateTimeKind.Local).AddTicks(338),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 888, DateTimeKind.Local).AddTicks(6960));
        }
    }
}
