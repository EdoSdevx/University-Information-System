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
    Task<ResultService<EnrollmentResponse>> EnrollStudentAsync(EnrollStudentRequest request);
    Task<ResultService> DropCourseAsync(DropCourseRequest request);
    Task<PagedResultService<StudentEnrollmentResponse>> GetCourseEnrollmentsAsync(int courseInstanceId, int pageIndex, int pageSize);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseInstanceId);
}