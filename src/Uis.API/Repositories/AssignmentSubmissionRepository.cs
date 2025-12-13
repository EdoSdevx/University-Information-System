using Microsoft.EntityFrameworkCore;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;

public class AssignmentSubmissionRepository : BaseRepository<AssignmentSubmission>, IAssignmentSubmissionRepository
{
    public AssignmentSubmissionRepository(ApplicationDbContext context) : base(context) {}

    public async Task<AssignmentSubmission?> GetStudentSubmissionAsync(int assignmentId, int studentId)
    {
        return await DbSet
                        .Include(s => s.Student)
                        .Include(s => s.Assignment)
                        .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
    }

    public async Task<List<AssignmentSubmission>> GetAssignmentSubmissionsAsync(int assignmentId)
    {
        return await DbSet
                        .Include(s => s.Student)
                        .Include(s => s.GradedByTeacher)
                        .Where(s => s.AssignmentId == assignmentId)
                        .OrderByDescending(s => s.SubmittedAt)
                        .ToListAsync();
    }
}