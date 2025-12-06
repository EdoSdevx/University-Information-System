using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs.Grade;

// ==================== REQUEST DTOs ====================
public class AssignGradeRequest
{
    public int StudentId { get; set; }
    public int CourseInstanceId { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Notes { get; set; }
}

public class UpdateGradeRequest
{
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Notes { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class GradeResponse
{
    public int Id { get; set; }
 
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? Section { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public DateTime? SubmittedAt { get; set; }
}

public class StudentGradeResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentEmail { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public DateTime? SubmittedAt { get; set; }
}