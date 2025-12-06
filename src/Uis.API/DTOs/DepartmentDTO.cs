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
}

public class UpdateDepartmentRequest
{
    public required string Name { get; set; }
    public required string Code { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class DepartmentResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class DepartmentDetailResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public int CourseCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}