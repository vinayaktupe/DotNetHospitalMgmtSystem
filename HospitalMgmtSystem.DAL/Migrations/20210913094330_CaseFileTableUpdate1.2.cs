using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgmtSystem.DAL.Migrations
{
    public partial class CaseFileTableUpdate12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 721, DateTimeKind.Local).AddTicks(3117),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(6631));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 721, DateTimeKind.Local).AddTicks(4678),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(7791));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CaseFiles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 713, DateTimeKind.Local).AddTicks(9177),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 888, DateTimeKind.Local).AddTicks(6960));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CaseFiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CasePapers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(6631),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 721, DateTimeKind.Local).AddTicks(3117));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseFiles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 893, DateTimeKind.Local).AddTicks(7791),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 721, DateTimeKind.Local).AddTicks(4678));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 9, 13, 10, 38, 2, 888, DateTimeKind.Local).AddTicks(6960),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 9, 13, 15, 13, 29, 713, DateTimeKind.Local).AddTicks(9177));
        }
    }
}
