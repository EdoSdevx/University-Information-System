using System;

namespace Uis.API.DTOs;

// ==================== REQUEST DTOs ====================
public class MarkAttendanceRequest
{
    public int EnrollmentId { get; set; }
    public int Week { get; set; }
    public string? Status { get; set; }
}

public class UpdateAttendanceRequest
{
    public string? Status { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class AttendanceResponse
{
    public int Id { get; set; }
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public int Week { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StudentAttendanceResponse
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public string? StudentName { get; set; }
    public int Week { get; set; }
    public string? Status { get; set; }
}