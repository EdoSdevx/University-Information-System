using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<List<Enrollment>> GetStudentEnrollmentsAsync(int studentId)
        {
            return await DbSet
                .Where(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active)
                .Include(e => e.CourseInstance)
                    .ThenInclude(ci => ci!.Course)
                .Include(e => e.CourseInstance)
                    .ThenInclude(ci => ci!.Teacher)
                .Include(e => e.CourseInstance)
                    .ThenInclude(ci => ci!.AcademicYear)
                .AsNoTracking()
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
        }
        public virtual async Task<bool> IsStudentEnrolledAsync(int studentId, int courseInstanceId)
        {
            return await DbSet
                .AnyAsync(e => e.StudentId == studentId &&
                              e.CourseInstanceId == courseInstanceId &&
                              e.Status == EnrollmentStatus.Active);
        }

        public virtual async Task<Enrollment?> GetEnrollmentAsync(int studentId, int courseInstanceId)
        {
            return await DbSet
                .FirstOrDefaultAsync(e => e.StudentId == studentId &&
                                          e.CourseInstanceId == courseInstanceId);
        }

        public virtual async Task<List<Enrollment>> GetCourseEnrollmentsAsync(int courseInstanceId)
        {
            return await DbSet
                .Where(e => e.CourseInstanceId == courseInstanceId && e.Status == EnrollmentStatus.Active)
                .Include(e => e.Student)
                .AsNoTracking()
                .OrderBy(e => e.Student!.FirstName)
                .ToListAsync();
        }

        public virtual async Task<int> GetCourseEnrollmentCountAsync(int courseInstanceId)
        {
            return await DbSet
                .CountAsync(e => e.CourseInstanceId == courseInstanceId &&
                                e.Status == EnrollmentStatus.Active);
        }
        public async Task<List<CourseInstructorDto>> GetStudentInstructorsAsync(int studentId)
        {
            return await DbSet
                .Include(e => e.CourseInstance)
                    .ThenInclude(ci => ci!.Course)
                .Include(e => e.CourseInstance)
                    .ThenInclude(ci => ci!.Teacher)
                .Where(e => e.StudentId == studentId)
                .Select(e => new CourseInstructorDto
                {
                    CourseCode = e.CourseInstance!.Course!.Code,
                    CourseName = e.CourseInstance!.Course.Name,
                    InstructorName = e.CourseInstance.Teacher != null
                        ? $"{e.CourseInstance.Teacher.FirstName} {e.CourseInstance.Teacher.LastName}"
                        : "TBA",
                    InstructorEmail = e.CourseInstance.Teacher != null
                        ? e.CourseInstance.Teacher.Email
                        : null
                })
                .ToListAsync();
        }
    }
}