using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uis.API.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecretaryEmail",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "SecretaryEmail",
                table: "Departments");
        }
    }
}
