using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HavucDent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiColumnToLaboratories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserMail",
                table: "Laboratories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Laboratories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Laboratories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserMail",
                table: "Laboratories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Laboratories",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserMail",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserMail",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Laboratories");
        }
    }
}
