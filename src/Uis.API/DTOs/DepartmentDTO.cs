using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs.Department;

// ==================== REQUEST DTOs ====================
public class CreateDepartmentRequest
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? Email { get; set; }
    public string? SecretaryEmail { get; set; }
    public string? DepartmentHeadName { get; set; }
    public string? DepartmentHeadEmail { get; set; }
}
public class UpdateDepartmentRequest
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Email { get; set; }
    public string? SecretaryEmail { get; set; }
    public string? DepartmentHeadName { get; set; }
    public string? DepartmentHeadEmail { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class DepartmentResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Email { get; set; }
    public string? SecretaryEmail { get; set; }
    public string? DepartmentHeadName { get; set; }
    public string? DepartmentHeadEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
public class DepartmentDetailResponse : DepartmentResponse
{
    public int CourseCount { get; set; }
    public int UserCount { get; set; }
}