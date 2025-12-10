using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;

public class Enrollment : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseInstanceId { get; set; }
    public int AcademicYearId { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    public DateTime EnrolledAt { get; set; }
    public DateTime? DroppedAt { get; set; }

    public User? Student { get; set; }
    public CourseInstance? CourseInstance { get; set; }
    public AcademicYear? AcademicYear { get; set; }

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}


