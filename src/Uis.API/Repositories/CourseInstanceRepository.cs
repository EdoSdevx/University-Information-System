using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class CourseInstanceRepository : BaseRepository<CourseInstance>, ICourseInstanceRepository
    {
        public CourseInstanceRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<List<CourseInstance>> GetStudentScheduleAsync(int studentId)
        {
            return await DbSet
                .Include(ci => ci.Course)
                .Include(ci => ci.Teacher)
                .Include(ci => ci.Department)
                .Include(ci => ci.Enrollments)
                .Where(ci => ci.Enrollments.Any(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active))
                .OrderBy(ci => ci.Day1)
                .ThenBy(ci => ci.StartTime)
                .ToListAsync();
        }
        public virtual async Task<List<CourseInstance>> GetAvailableCoursesAsync(int academicYearId)
        {
            return await DbSet
                .Where(ci => ci.AcademicYearId == academicYearId &&
                             ci.Status == CourseInstanceStatus.Open)
                .Include(ci => ci.Enrollments)
                .Include(ci => ci.Course)
                .Include(ci => ci.Teacher)
                .AsNoTracking()
                .OrderBy(ci => ci.Course!.Code)
                .ToListAsync();
        }
        public virtual async Task<CourseInstance?> GetForEnrollmentAsync(int courseInstanceId)
        {
            return await DbSet
                .Include(ci => ci.Course)
                .Include(ci => ci.Teacher)
                .Include(ci => ci.AcademicYear)
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);
        }

        public virtual async Task<bool> HasCapacityAsync(int courseInstanceId)
        {
            var instance = await DbSet.AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);

            if (instance == null)
                return false;

            return instance.CurrentEnrollmentCount < instance.Capacity;
        }
        public virtual async Task<List<CourseInstance>> GetTeacherCoursesAsync(int teacherId)
        {
            return await DbSet
                .Where(ci => ci.TeacherId == teacherId)
                .Include(ci => ci.Course)
                .Include(ci => ci.Enrollments)
                .Include(ci => ci.AcademicYear)
                .AsNoTracking()
                .OrderByDescending(ci => ci.AcademicYear!.StartYear)
                .ToListAsync();
        }
        public virtual async Task<CourseInstance?> GetWithEnrollmentsAsync(int courseInstanceId)
        {
            return await DbSet
                .Include(ci => ci.Enrollments)
                .FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);
        }

        public async Task<List<CourseInstance>> GetAllWithDetailsAsync()
        {
            return await DbSet
                            .Include(ci => ci.Course)
                            .Include(ci => ci.Teacher)
                            .Include(ci => ci.AcademicYear)
                            .Include(ci => ci.Department)
                            .Include(ci => ci.Enrollments)
                            .ToListAsync();
        }

        public async Task<CourseInstance?> GetByIdWithDetailsAsync(int id)
        {
            return await DbSet
                            .Include(ci => ci.Course)
                                .ThenInclude(c => c!.Department)
                            .Include(ci => ci.Teacher)
                            .Include(ci => ci.AcademicYear)
                            .Include(ci => ci.Department)
                            .Include(ci => ci.Enrollments)
                            .FirstOrDefaultAsync(ci => ci.Id == id);
        }
    }
}