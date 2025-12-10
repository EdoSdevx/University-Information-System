using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces;
public interface IAttendanceRepository : IBaseRepository<Attendance>
{
    Task<List<Attendance>> GetStudentAttendanceAsync(int studentId);
    Task<List<Attendance>> GetCourseAttendanceAsync(int courseInstanceId, DateTime date);
    Task<Attendance?> GetAttendanceAsync(int enrollmentId, DateTime date);
}
