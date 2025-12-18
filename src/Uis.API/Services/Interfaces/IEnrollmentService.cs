using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Enrollment;

namespace Uis.API.Services.Interfaces;

public interface IEnrollmentService
{
    Task<PagedResultService<EnrollmentResponse>> GetStudentEnrollmentsAsync(int studentId, int pageIndex, int pageSize);
    Task<ResultService<EnrollmentResponse>> EnrollStudentAsync(int studentId, EnrollStudentRequest request);
    Task<ResultService> DropCourseAsync(int studentId, DropCourseRequest request);
    Task<PagedResultService<StudentEnrollmentResponse>> GetCourseEnrollmentsAsync(int courseInstanceId, int pageIndex, int pageSize, int teacherId, bool byPass);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseInstanceId);

    Task<ResultService<EnrollmentResponse>> AdminEnrollStudentAsync(AdminEnrollStudentRequest request);
    Task<ResultService> AdminDropCourseAsync(AdminDropCourseRequest request);

    // PRIVATE ExecuteEnrollAsync
}