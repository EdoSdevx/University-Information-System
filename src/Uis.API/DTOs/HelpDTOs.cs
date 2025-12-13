using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs;
public class HelpContactsResponse
{
    public string? DepartmentName { get; set; }
    public string? DepartmentEmail { get; set; }
    public string? DepartmentSecretaryEmail { get; set; }
    public string? StudentAffairsEmail { get; set; }
    public string? RegistrarEmail { get; set; }
    public string? ITSupportEmail { get; set; }
    public List<CourseInstructorDto>? CourseInstructors { get; set; }
}

public class CourseInstructorDto
{
    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
    public string? InstructorName { get; set; }
    public string? InstructorEmail { get; set; }
}