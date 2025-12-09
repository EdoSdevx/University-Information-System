using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;
public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseInstanceId { get; set; }
    public int SubmittedByTeacherId { get; set; }

    public int? Exam1 { get; set; }
    public int? Exam2 { get; set; }
    public int? Final { get; set; }
    public int? Project { get; set; }

    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Audit trail
    public string? Notes { get; set; }

    public virtual User? Student { get; set; }
    public virtual CourseInstance? CourseInstance { get; set; }
    public virtual User? SubmittedByTeacher { get; set; }
    public virtual AcademicYear? AcademicYear { get; set; }
}
