using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Grade;

namespace Uis.API.Services.Interfaces;

public interface IGradeService
{
    Task<PagedResultService<GradeResponse>> GetStudentGradesAsync(int studentId, int pageIndex, int pageSize);
    Task<ResultService<GradeResponse>> GetStudentCourseGradeAsync(int studentId, int courseInstanceId);
    Task<PagedResultService<StudentGradeResponse>> GetCourseGradesAsync(int studentId, int pageIndex, int pageSize);
    Task<ResultService<GradeResponse>> AssignGradeAsync(AssignGradeRequest request);
    Task<ResultService<GradeResponse>> UpdateGradeAsync(int gradeId, int teacherId, UpdateGradeRequest request);
    Task<ResultService> DeleteGradeAsync(int gradeId, int teacherId);
}