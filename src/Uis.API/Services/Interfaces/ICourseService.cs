using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Course;

namespace Uis.API.Services.Interfaces;

public interface ICourseService
{
    Task<ResultService<CourseResponse>> GetCourseByCodeAsync(string code);
    Task<PagedResultService<CourseResponse>> GetByDepartmentAsync(int departmentId, int pageIndex, int pageSize);
    Task<PagedResultService<CourseResponse>> GetAllCoursesAsync(int pageIndex, int pageSize);
}
