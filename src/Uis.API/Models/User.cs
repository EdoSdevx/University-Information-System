using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;

public class User : BaseEntity
{

    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }

    public string? Major { get; set; }
    public string? AcademicYear { get; set; }
    public string? OfficeLocation { get; set; }

    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelationship { get; set; }

    public int DepartmentId { get; set; }
    public virtual Department? Department { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<Enrollment> StudentEnrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<CourseInstance> TaughtCourses { get; set; } = new List<CourseInstance>();
    public virtual ICollection<Grade> GradesSubmitted { get; set; } = new List<Grade>();
    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}