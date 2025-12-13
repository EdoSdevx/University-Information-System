using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;
public class CourseInstance : BaseEntity

{
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int AcademicYearId { get; set; }
    public int DepartmentId { get; set; }

    public required string Section { get; set; }
    public int Capacity { get; set; }
    public int CurrentEnrollmentCount { get; set; }
    public DayOfWeek? Day1 { get; set; } 
    public DayOfWeek? Day2 { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public required string Location { get; set; }
    public CourseInstanceStatus Status { get; set; } = CourseInstanceStatus.Open;

    public byte[]? ConcurrencyToken { get; set; }

    public virtual Course? Course { get; set; }
    public virtual User? Teacher { get; set; }
    public virtual AcademicYear? AcademicYear { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
