using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanVidly.Migrations
{
    public partial class IncreaseUserSaltLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldMaxLength: 64);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldMaxLength: 128);
        }
    }
}
