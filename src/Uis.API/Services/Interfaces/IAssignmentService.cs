using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs;

namespace Uis.API.Services.Interfaces;
public interface IAssignmentService
{
    Task<ResultService<List<AssignmentResponse>>> GetTeacherAssignmentsAsync(int teacherId, int courseInstanceId);
    Task<ResultService<AssignmentResponse>> CreateAssignmentAsync(int teacherId, CreateAssignmentRequest request);
    Task<ResultService<AssignmentResponse>> UpdateAssignmentAsync(int assignmentId, int teacherId, UpdateAssignmentRequest request);
    Task<ResultService> DeleteAssignmentAsync(int assignmentId, int teacherId);
    Task<ResultService<List<SubmissionResponse>>> GetAssignmentSubmissionsAsync(int assignmentId, int teacherId);
    Task<ResultService<List<StudentAssignmentResponse>>> GetStudentAssignmentsAsync(int studentId);
    Task<ResultService> GradeSubmissionAsync(int submissionId, int teacherId, GradeSubmissionRequest request);
    Task<ResultService> SubmitAssignmentAsync(int studentId, int assignmentId, SubmitAssignmentDto model);
}
