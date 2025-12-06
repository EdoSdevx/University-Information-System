using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs.AcademicYear;

// ==================== REQUEST DTOs ====================
public class AcademicYearRequest
{
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public DateTime EnrollmentStartDate { get; set; }
    public DateTime EnrollmentEndDate { get; set; }
    public bool IsActive { get; set; }
}

// ==================== RESPONSE DTOs ====================
public class AcademicYearResponse
{
    public int Id { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public DateTime EnrollmentStartDate { get; set; }
    public DateTime EnrollmentEndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
