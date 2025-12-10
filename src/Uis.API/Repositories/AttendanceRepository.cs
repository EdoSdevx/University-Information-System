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
            .OrderByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<List<Attendance>> GetCourseAttendanceAsync(int courseInstanceId, DateTime date)
    {
        return await DbSet
            .Where(a => a.Enrollment!.CourseInstanceId == courseInstanceId &&
                       a.AttendanceDate.Date == date.Date)
            .Include(a => a.Enrollment)
                .ThenInclude(e => e.Student)
            .OrderBy(a => a.Enrollment!.Student!.FirstName)
            .ToListAsync();
    }

    public async Task<Attendance?> GetAttendanceAsync(int enrollmentId, DateTime date)
    {
        return await DbSet
            .FirstOrDefaultAsync(a => a.EnrollmentId == enrollmentId && a.AttendanceDate.Date == date.Date);
    }
}
