// Services/AssignmentService.cs
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignmentService> _logger;

    public AssignmentService(IUnitOfWork unitOfWork, ILogger<AssignmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultService<List<AssignmentResponse>>> GetTeacherAssignmentsAsync(int teacherId, int courseInstanceId)
    {
        _logger.LogInformation("Fetching assignments for teacher {TeacherId} in course {CourseInstanceId}",
            teacherId, courseInstanceId);

        var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(courseInstanceId);
        if (courseInstance == null)
        {
            _logger.LogWarning("Course instance {CourseInstanceId} not found", courseInstanceId);
            return ResultService<List<AssignmentResponse>>.NotFound("Course not found");
        }

        if (courseInstance.TeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for course {CourseInstanceId}",
                teacherId, courseInstanceId);
            return ResultService<List<AssignmentResponse>>.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        var assignments = await _unitOfWork.Assignments.GetTeacherAssignmentsAsync(teacherId, courseInstanceId);

        var dtos = assignments.Select(a => new AssignmentResponse
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            DueDate = a.DueDate,
            TotalPoints = a.TotalPoints,
            Status = a.Status.ToString(),
            CourseCode = a.CourseInstance?.Course?.Code,
            CourseName = a.CourseInstance?.Course?.Name,
            SubmissionsCount = a.Submissions?.Count ?? 0,
            GradedCount = a.Submissions?.Count(s => s.Status == SubmissionStatus.Graded) ?? 0,
            CreatedAt = a.CreatedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} assignments for teacher {TeacherId}",
            dtos.Count, teacherId);

        return ResultService<List<AssignmentResponse>>.Ok(dtos);
    }

    public async Task<ResultService<AssignmentResponse>> CreateAssignmentAsync(int teacherId, CreateAssignmentRequest request)
    {
        _logger.LogInformation("Creating assignment for teacher {TeacherId} in course {CourseInstanceId}",
            teacherId, request?.CourseInstanceId);

        if (request == null)
            return ResultService<AssignmentResponse>.Fail("Assignment data required");

        var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(request.CourseInstanceId);
        if (courseInstance == null)
        {
            _logger.LogWarning("Course instance {CourseInstanceId} not found", request.CourseInstanceId);
            return ResultService<AssignmentResponse>.NotFound("Course not found");
        }

        if (courseInstance.TeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for course {CourseInstanceId}",
                teacherId, request.CourseInstanceId);
            return ResultService<AssignmentResponse>.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        var assignment = new Assignment
        {
            CourseInstanceId = request.CourseInstanceId,
            CreatedByTeacherId = teacherId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            TotalPoints = request.TotalPoints,
            Status = AssignmentStatus.Published,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Assignments.AddAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        var dto = new AssignmentResponse
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description,
            DueDate = assignment.DueDate,
            TotalPoints = assignment.TotalPoints,
            Status = assignment.Status.ToString(),
            CourseCode = courseInstance.Course?.Code,
            CourseName = courseInstance.Course?.Name,
            SubmissionsCount = 0,
            GradedCount = 0,
            CreatedAt = assignment.CreatedAt
        };

        _logger.LogInformation("Assignment {AssignmentId} created successfully", assignment.Id);

        return ResultService<AssignmentResponse>.Ok(dto, "Assignment created successfully");
    }

    public async Task<ResultService<AssignmentResponse>> UpdateAssignmentAsync(int assignmentId, int teacherId, UpdateAssignmentRequest request)
    {
        _logger.LogInformation("Updating assignment {AssignmentId} by teacher {TeacherId}",
            assignmentId, teacherId);

        if (request == null)
            return ResultService<AssignmentResponse>.Fail("Assignment data required");

        var assignment = await _unitOfWork.Assignments.GetAssignmentWithSubmissionsAsync(assignmentId);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            return ResultService<AssignmentResponse>.NotFound("Assignment not found");
        }

        if (assignment.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for assignment {AssignmentId}",
                teacherId, assignmentId);
            return ResultService<AssignmentResponse>.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        if (!string.IsNullOrEmpty(request.Title))
            assignment.Title = request.Title;
        if (!string.IsNullOrEmpty(request.Description))
            assignment.Description = request.Description;
        if (request.DueDate.HasValue)
            assignment.DueDate = request.DueDate.Value;
        if (request.TotalPoints.HasValue)
            assignment.TotalPoints = request.TotalPoints.Value;

        assignment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Assignments.UpdateAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        var dto = new AssignmentResponse
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description,
            DueDate = assignment.DueDate,
            TotalPoints = assignment.TotalPoints,
            Status = assignment.Status.ToString(),
            CourseCode = assignment.CourseInstance?.Course?.Code,
            CourseName = assignment.CourseInstance?.Course?.Name,
            SubmissionsCount = assignment.Submissions?.Count ?? 0,
            GradedCount = assignment.Submissions?.Count(s => s.Status == SubmissionStatus.Graded) ?? 0,
            CreatedAt = assignment.CreatedAt
        };

        _logger.LogInformation("Assignment {AssignmentId} updated successfully", assignmentId);

        return ResultService<AssignmentResponse>.Ok(dto, "Assignment updated successfully");
    }

    public async Task<ResultService> DeleteAssignmentAsync(int assignmentId, int teacherId)
    {
        _logger.LogInformation("Deleting assignment {AssignmentId} by teacher {TeacherId}",
            assignmentId, teacherId);

        var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            return ResultService.NotFound("Assignment not found");
        }

        if (assignment.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for assignment {AssignmentId}",
                teacherId, assignmentId);
            return ResultService.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        await _unitOfWork.Assignments.DeleteAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Assignment {AssignmentId} deleted successfully", assignmentId);

        return ResultService.Ok("Assignment deleted successfully");
    }

    public async Task<ResultService<List<SubmissionResponse>>> GetAssignmentSubmissionsAsync(int assignmentId, int teacherId)
    {
        _logger.LogInformation("Fetching submissions for assignment {AssignmentId} by teacher {TeacherId}",
            assignmentId, teacherId);

        var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);

        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            return ResultService<List<SubmissionResponse>>.NotFound("Assignment not found");
        }

        if (assignment.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for assignment {AssignmentId}",
                teacherId, assignmentId);
            return ResultService<List<SubmissionResponse>>.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        var submissions = await _unitOfWork.AssignmentSubmissions.GetAssignmentSubmissionsAsync(assignmentId);

        var dtos = submissions.Select(s => new SubmissionResponse
        {
            Id = s.Id,
            AssignmentId = s.AssignmentId,
            StudentId = s.StudentId,
            StudentName = $"{s.Student?.FirstName} {s.Student?.LastName}",
            StudentEmail = s.Student?.Email,
            FileUrl = s.FileUrl,
            SubmissionText = s.SubmissionText,
            SubmittedAt = s.SubmittedAt,
            Grade = s.Grade,
            Status = s.Status.ToString()
        }).ToList();

        _logger.LogInformation("Retrieved {Count} submissions for assignment {AssignmentId}",
            dtos.Count, assignmentId);

        foreach(SubmissionResponse test in dtos)
        {
            _logger.LogInformation(test.FileUrl);
        }
        return ResultService<List<SubmissionResponse>>.Ok(dtos);
    }

    public async Task<ResultService<List<StudentAssignmentResponse>>> GetStudentAssignmentsAsync(int studentId)
    {
        _logger.LogInformation("Fetching assignments for student {StudentId}", studentId);

        var assignments = await _unitOfWork.Assignments.GetStudentAssignmentsAsync(studentId);

        var dtos = assignments.Select(a =>
        {
            var submission = a.Submissions?.FirstOrDefault(s => s.StudentId == studentId);

            return new StudentAssignmentResponse
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                TotalPoints = a.TotalPoints,
                CourseCode = a.CourseInstance?.Course?.Code ?? "N/A",
                CourseName = a.CourseInstance?.Course?.Name ?? "N/A",
                TeacherName = a.CourseInstance?.Teacher != null
                    ? $"{a.CourseInstance.Teacher.FirstName} {a.CourseInstance.Teacher.LastName}"
                    : "N/A",
                IsSubmitted = submission != null && submission.SubmittedAt.HasValue,
                Grade = submission?.Grade,
                SubmittedAt = submission?.SubmittedAt
            };
        }).ToList();

        _logger.LogInformation("Retrieved {Count} assignments for student {StudentId}",
            dtos.Count, studentId);

        return ResultService<List<StudentAssignmentResponse>>.Ok(dtos);
    }

    public async Task<ResultService> GradeSubmissionAsync(int submissionId, int teacherId, GradeSubmissionRequest request)
    {
        _logger.LogInformation("Grading submission {SubmissionId} by teacher {TeacherId}",
            submissionId, teacherId);

        if (request == null)
            return ResultService.Fail("Grade data required");

        var submission = await _unitOfWork.AssignmentSubmissions.GetByIdAsync(submissionId);
        if (submission == null)
        {
            _logger.LogWarning("Submission {SubmissionId} not found", submissionId);
            return ResultService.NotFound("Submission not found");
        }

        var assignment = await _unitOfWork.Assignments.GetByIdAsync(submission.AssignmentId);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", submission.AssignmentId);
            return ResultService.NotFound("Assignment not found");
        }

        if (assignment.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} not authorized for submission {SubmissionId}",
                teacherId, submissionId);
            return ResultService.Fail("Not authorized", ResultErrorCode.Forbidden);
        }

        submission.Grade = request.Grade;
        submission.GradedAt = DateTime.UtcNow;
        submission.GradedByTeacherId = teacherId;
        submission.Status = SubmissionStatus.Graded;
        submission.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.AssignmentSubmissions.UpdateAsync(submission);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Submission {SubmissionId} graded successfully with grade {Grade}",
            submissionId, request.Grade);

        return ResultService.Ok("Submission graded successfully");
    }

    public async Task<ResultService> SubmitAssignmentAsync(int studentId, int assignmentId, SubmitAssignmentDto model)
    {
        _logger.LogInformation("Student {StudentId} submitting assignment {AssignmentId}", studentId, assignmentId);

        if (string.IsNullOrWhiteSpace(model.SubmissionText))
        {
            _logger.LogWarning("Submission text is required");
            return ResultService.Fail("Submission text is required");
        }

        var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            return ResultService.NotFound("Assignment not found");
        }

        if (assignment.Status != AssignmentStatus.Published)
        {
            _logger.LogWarning("Assignment {AssignmentId} is not published", assignmentId);
            return ResultService.Fail("This assignment is not available for submission");
        }

        if (assignment.DueDate < DateTime.UtcNow)
        {
            _logger.LogWarning("Assignment {AssignmentId} is past due date", assignmentId);
            return ResultService.Fail("This assignment is past the due date");
        }

        var isEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(studentId, assignment.CourseInstanceId);
        if (!isEnrolled)
        {
            _logger.LogWarning("Student {StudentId} is not enrolled in course", studentId);
            return ResultService.Fail("You are not enrolled in this course", ResultErrorCode.Forbidden);
        }

        var existingSubmission = await _unitOfWork.AssignmentSubmissions.GetStudentSubmissionAsync(assignmentId, studentId);

        if (existingSubmission != null)
        {
            _logger.LogWarning("Student {StudentId} already submitted assignment {AssignmentId}", studentId, assignmentId);
            return ResultService.Fail("You have already submitted this assignment", ResultErrorCode.Conflict);
        }

        var submission = new AssignmentSubmission
        {
            AssignmentId = assignmentId,
            StudentId = studentId,
            SubmissionText = model.SubmissionText,
            FileUrl = model.FileUrl,
            SubmittedAt = DateTime.UtcNow,
            Status = SubmissionStatus.Submitted,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.AssignmentSubmissions.AddAsync(submission);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Assignment {AssignmentId} submitted successfully by student {StudentId}", assignmentId, studentId);

        return ResultService.Ok("Assignment submitted successfully");
    }
}