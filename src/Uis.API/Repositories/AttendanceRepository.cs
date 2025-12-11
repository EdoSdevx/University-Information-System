using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;

public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context) {}

    public async Task<List<Attendance>> GetStudentAttendanceAsync(int studentId)
    {
        return await DbSet
            .Where(a => a.Enrollment!.StudentId == studentId)
            .Include(a => a.Enrollment)
                .ThenInclude(e => e.CourseInstance)
                    .ThenInclude(ci => ci.Course)
            .OrderByDescending(a => a.Week)
            .ToListAsync();
    }

    public async Task<List<Attendance>> GetCourseAttendanceAsync(int courseInstanceId, int week)
    {
        return await DbSet
            .Where(a => a.Enrollment!.CourseInstanceId == courseInstanceId &&
                       a.Week == week)
            .Include(a => a.Enrollment)
                .ThenInclude(e => e.Student)
            .OrderBy(a => a.Enrollment!.Student!.FirstName)
            .ToListAsync();
    }

    public async Task<Attendance?> GetAttendanceAsync(int enrollmentId, int week)
    {
        return await DbSet
            .FirstOrDefaultAsync(a => a.EnrollmentId == enrollmentId && a.Week == week);
    }
}
