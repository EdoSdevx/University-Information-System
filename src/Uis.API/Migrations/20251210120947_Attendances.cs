using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uis.API.Migrations
{
    /// <inheritdoc />
    public partial class Attendances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendances_EnrollmentId",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendances",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Date",
                table: "Attendances",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Enrollment_Date_Unique",
                table: "Attendances",
                columns: new[] { "EnrollmentId", "AttendanceDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendance_Date",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_Enrollment_Date_Unique",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EnrollmentId",
                table: "Attendances",
                column: "EnrollmentId");
        }
    }
}
