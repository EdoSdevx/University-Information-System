using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;
public class Announcement : BaseEntity
{
    public int CreatedByTeacherId { get; set; }
    public string? CreatedByTeacherName { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedAt { get; set; }

    public int? TargetAcademicYearId { get; set; }
    public int? TargetCourseInstanceId { get; set; }

    public virtual User? CreatedByTeacher { get; set; } 
    public virtual Department? TargetDepartment { get; set; } 
    public virtual AcademicYear? TargetAcademicYear { get; set; } 
    public virtual CourseInstance? TargetCourseInstance { get; set; } 
}

