using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs;

public class AssignmentResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int TotalPoints { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public int SubmissionsCount { get; set; }
    public int GradedCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAssignmentRequest
{
    public int CourseInstanceId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int TotalPoints { get; set; }
}

public class UpdateAssignmentRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? TotalPoints { get; set; }
}

public class SubmissionResponse
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string? StudentEmail { get; set; }
    public string? SubmissionText { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? Grade { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class GradeSubmissionRequest
{
    public int Grade { get; set; }
}

public class StudentAssignmentResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int TotalPoints { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public bool IsSubmitted { get; set; }
    public int? Grade { get; set; }
    public DateTime? SubmittedAt { get; set; }
}
public class SubmitAssignmentRequest
{
    public int AssignmentId { get; set; }
    public string? SubmissionText { get; set; }
    public IFormFile? File { get; set; }
}

public class SubmitAssignmentDto
{
    public string SubmissionText { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
}