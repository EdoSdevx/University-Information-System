using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uis.API.Migrations
{
    /// <inheritdoc />
    public partial class AttendancesDay12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendance_Enrollment_Week_Unique",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Enrollment_Week_Day_Unique",
                table: "Attendances",
                columns: new[] { "EnrollmentId", "Week", "Day" },
                unique: true,
                filter: "[Day] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendance_Enrollment_Week_Day_Unique",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Enrollment_Week_Unique",
                table: "Attendances",
                columns: new[] { "EnrollmentId", "Week" },
                unique: true);
        }
    }
}
