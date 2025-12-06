using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;

public class Course : BaseEntity
{
    public required string Code { get; set; } 
    public required string Name { get; set; }

    public int CreditHours { get; set; }
    public int DepartmentId { get; set; }

    public int? PrerequisiteCourseId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Course? PrerequisiteCourse { get; set; }
    public virtual ICollection<CourseInstance> Instances { get; set; } = new List<CourseInstance>();
}