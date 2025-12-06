using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface IGradeRepository : IBaseRepository<Grade>
    {
        Task<List<Grade>> GetStudentGradesAsync(int studentId);
        Task<Grade?> GetStudentCourseGradeAsync(int studentId, int courseInstanceId);
        Task<List<Grade>> GetCourseGradesAsync(int courseInstanceId);
        Task<int> GetCourseGradeCountAsync(int courseInstanceId);
        Task<bool> StudentHasGradeAsync(int studentId, int courseInstanceId);
    }
}