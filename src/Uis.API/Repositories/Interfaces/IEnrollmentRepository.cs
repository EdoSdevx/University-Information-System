using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IBaseRepository<Enrollment>
    {
        Task<List<Enrollment>> GetStudentEnrollmentsAsync(int studentId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseInstanceId);
        Task<Enrollment?> GetEnrollmentAsync(int studentId, int courseInstanceId);
        Task<List<Enrollment>> GetCourseEnrollmentsAsync(int courseInstanceId);
        Task<int> GetCourseEnrollmentCountAsync(int courseInstanceId);

    }
}