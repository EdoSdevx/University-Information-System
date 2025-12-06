using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface ICourseInstanceRepository : IBaseRepository<CourseInstance>
    {
        Task<List<CourseInstance>> GetStudentScheduleAsync(int studentId);
        Task<List<CourseInstance>> GetAvailableCoursesAsync(int academicYearId);
        Task<CourseInstance?> GetForEnrollmentAsync(int courseInstanceId);
        Task<bool> HasCapacityAsync(int courseInstanceId);
        Task<List<CourseInstance>> GetTeacherCoursesAsync(int teacherId);
        Task<CourseInstance?> GetWithEnrollmentsAsync(int courseInstanceId);
    }
}