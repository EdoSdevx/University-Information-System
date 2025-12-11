using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs;

namespace Uis.API.Services.Interfaces;
public interface IAttendanceService
{
    Task<ResultService> MarkAttendanceAsync(int enrollmentId, MarkAttendanceRequest request);
    Task<PagedResultService<AttendanceResponse>> GetStudentAttendanceAsync(int studentId, int pageIndex, int pageSize);
    Task<PagedResultService<StudentAttendanceResponse>> GetCourseAttendanceAsync(int courseInstanceId, int wek, int pageIndex, int pageSize);
    Task<ResultService> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceRequest request);
    Task<ResultService> DeleteAttendanceAsync(int attendanceId);
}
