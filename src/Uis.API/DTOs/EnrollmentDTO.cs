using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.DTOs.Enrollment;

// ==================== REQUEST DTOs ====================
public class EnrollStudentRequest
{
    public int StudentId { get; set; }
    public int CourseInstanceId { get; set; }
}

public class DropCourseRequest
{
    public int StudentId { get; set; }
    public int CourseInstanceId { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class EnrollmentResponse
{
    public int Id { get; set; }
    public int CourseInstanceId { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? Section { get; set; }
    public string? TeacherName { get; set; }
    public string? AcademicYear { get; set; }
    public string? Status { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? DroppedAt { get; set; }
}

public class StudentEnrollmentResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentEmail { get; set; }
    public DateTime EnrolledAt { get; set; }
}