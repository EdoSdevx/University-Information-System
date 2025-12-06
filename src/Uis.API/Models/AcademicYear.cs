using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;

public class AcademicYear : BaseEntity
{
    public required string Year { get; set; } 
    public int StartYear { get; set; } 
    public int EndYear { get; set; } 
    public DateTime EnrollmentStartDate { get; set; }
    public DateTime EnrollmentEndDate { get; set; }
    public bool IsActive { get; set; }

}
