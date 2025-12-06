using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.CourseInstance;
using Uis.API.Models;

namespace Uis.API.Services.Interfaces;

public interface ICourseInstanceService
{
    Task<ResultService<List<CourseInstanceSchedule>>> GetStudentScheduleAsync(int studentId);
    Task<PagedResultService<CourseInstanceResponse>> GetAvailableCoursesAsync(int studentId, int academicYearId, int pageIndex = 1, int pageSize = 10);
    Task<ResultService<CourseInstanceResponse>> GetForEnrollmentAsync(int courseInstanceId);
    Task<ResultService<bool>> HasCapacityAsync(int courseInstanceId);
    Task<PagedResultService<CourseInstanceResponse>> GetTeacherCoursesAsync(int teacherId, int pageIndex = 1, int pageSize = 10);

}