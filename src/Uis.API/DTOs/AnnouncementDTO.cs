using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.DTOs.Announcement;

// ==================== REQUEST DTOs ====================
public class CreateAnnouncementRequest
{
    public int CreatedByTeacherId { get; set; }
    public int TargetCourseInstanceId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

}
public class UpdateAnnouncementRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }

}

// ==================== RESPONSE DTOs ====================
public class AnnouncementResponse
{
    public int Id { get; set; }
    public int TargetCourseInstanceId { get; set; }
    public int CreatedByTeacherId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? CreatedByTeacherName { get; set; }
    public DateTime? PublishedAt { get; set; }

    public string? CourseCode { get; set; }
    public string? CourseName { get; set; }
}

