using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class GradeRepository : BaseRepository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<List<Grade>> GetStudentGradesAsync(int studentId)
        {
            return await DbSet
                .Where(g => g.StudentId == studentId)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Teacher)
                .AsNoTracking()
                .OrderByDescending(g => g.SubmittedAt)
                .ToListAsync();
        }

        public virtual async Task<Grade?> GetStudentCourseGradeAsync(int studentId, int courseInstanceId)
        {
            return await DbSet
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .FirstOrDefaultAsync(g => g.StudentId == studentId &&
                                         g.CourseInstanceId == courseInstanceId);
        }
        public virtual async Task<List<Grade>> GetCourseGradesAsync(int courseInstanceId)
        {
            return await DbSet
                .Where(g => g.CourseInstanceId == courseInstanceId)
                .Include(g => g.Student)
                .AsNoTracking()
                .OrderBy(g => g.Student.LastName)
                .ThenBy(g => g.Student.FirstName)
                .ToListAsync();
        }

        public virtual async Task<int> GetCourseGradeCountAsync(int courseInstanceId)
        {
            return await DbSet
                .CountAsync(g => g.CourseInstanceId == courseInstanceId);
        }

        public virtual async Task<bool> StudentHasGradeAsync(int studentId, int courseInstanceId)
        {
            return await DbSet
                .AnyAsync(g => g.StudentId == studentId &&
                              g.CourseInstanceId == courseInstanceId);
        }
    }
}