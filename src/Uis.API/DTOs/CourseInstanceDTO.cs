using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.DTOs.CourseInstance;

// ==================== REQUEST DTOs ====================
public class CreateCourseInstanceRequest
{
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int AcademicYearId { get; set; }
    public string? Section { get; set; }
    public int Capacity { get; set; }
    public DayOfWeek? Day1 { get; set; }
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Location { get; set; }
}

public class UpdateCourseInstanceRequest
{
    public string? Section { get; set; }
    public int Capacity { get; set; }
    public DayOfWeek? Day1 { get; set; }
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Location { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class CourseInstanceSchedule
{
    public int CourseInstanceId { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
    public string? Section { get; set; }
    public DayOfWeek? Day1 { get; set; }
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Location { get; set; }
}

public class CourseInstanceResponse
{
    public int CourseInstanceId { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
    public string? AcademicYear { get; set; }
    public string? Section { get; set; }
    public int Capacity { get; set; }
    public int CurrentEnrollmentCount { get; set; }
    public DayOfWeek? Day1 { get; set; }
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Location { get; set; }
    public CourseInstanceStatus Status { get; set; }
}

public class CourseInstanceDetailResponse
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? CourseDescription { get; set; }
    public int CreditHours { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherFirstName { get; set; }
    public string? TeacherLastName { get; set; }
    public int AcademicYearId { get; set; }
    public string? AcademicYear { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentCode { get; set; }
    public string? Section { get; set; }
    public int Capacity { get; set; }
    public int CurrentEnrollmentCount { get; set; }
    public int AvailableSeats { get; set; }
    public DayOfWeek? Day1 { get; set; }
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Location { get; set; }
    public CourseInstanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}