using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;
public class AssignmentSubmission : BaseEntity
{
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }

    public string? SubmissionText { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? Grade { get; set; }
    public DateTime? GradedAt { get; set; }
    public int? GradedByTeacherId { get; set; }
    public SubmissionStatus Status { get; set; }

    public virtual Assignment? Assignment { get; set; }
    public virtual User? Student { get; set; }
    public virtual User? GradedByTeacher { get; set; }

}
