using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs.Course;

// ==================== REQUEST DTOs ====================
public class CreateCourseRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int CreditHours { get; set; }
    public int DepartmentId { get; set; }
    public int? PrerequisiteCourseId { get; set; }
}

public class UpdateCourseRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int CreditHours { get; set; }
    public int DepartmentId { get; set; }
    public int? PrerequisiteCourseId { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class CourseResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int CreditHours { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CourseDetailResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int CreditHours { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentCode { get; set; }
    public string? DepartmentName { get; set; }
    public int? PrerequisiteCourseId { get; set; }
    public string? PrerequisiteCode { get; set; }
    public int CourseInstanceCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}