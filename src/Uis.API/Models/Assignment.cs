using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;
public class Assignment : BaseEntity
{
    public int CourseInstanceId { get; set; }
    public int CreatedByTeacherId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int TotalPoints { get; set; }
    public AssignmentStatus Status { get; set; }

    public virtual CourseInstance? CourseInstance { get; set; }
    public virtual User? CreatedByTeacher { get; set; }
    public virtual ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

