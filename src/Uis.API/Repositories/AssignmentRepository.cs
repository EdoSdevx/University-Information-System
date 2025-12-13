using Microsoft.EntityFrameworkCore;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context) {}

    public async Task<List<Assignment>> GetTeacherAssignmentsAsync(int teacherId, int courseInstanceId)
    {
        return await DbSet
            .Include(a => a.CourseInstance)
                .ThenInclude(ci => ci.Course)
            .Include(a => a.Submissions)
            .Where(a => a.CreatedByTeacherId == teacherId && a.CourseInstanceId == courseInstanceId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Assignment>> GetStudentAssignmentsAsync(int studentId)
    {
        return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .SelectMany(e => e.CourseInstance.Assignments)
            .Include(a => a.CourseInstance)
                .ThenInclude(ci => ci.Course)
            .Include(a => a.CourseInstance)
                .ThenInclude(ci => ci.Teacher)
            .Include(a => a.Submissions.Where(s => s.StudentId == studentId))
            .OrderBy(a => a.DueDate)
            .ToListAsync();
    }

    public async Task<Assignment?> GetAssignmentWithSubmissionsAsync(int assignmentId)
    {
        return await DbSet
            .Include(a => a.CourseInstance)
                .ThenInclude(ci => ci.Course)
            .Include(a => a.Submissions)
                .ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
    }
}