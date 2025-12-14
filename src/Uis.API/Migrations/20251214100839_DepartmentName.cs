using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uis.API.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentHeadEmail",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentHeadName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentHeadEmail",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentHeadName",
                table: "Departments");
        }
    }
}
