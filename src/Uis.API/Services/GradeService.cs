using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Grade;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class GradeService : IGradeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GradeService> _logger;

    public GradeService(IUnitOfWork unitOfWork, ILogger<GradeService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<PagedResultService<GradeResponse>> GetStudentGradesAsync(int studentId, int pageIndex = 1, int pageSize = 10)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var student = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", studentId);
            return PagedResultService<GradeResponse>.Fail("Student not found");
        }

        var grades = await _unitOfWork.Grades.GetStudentGradesAsync(studentId);

        var totalCount = grades.Count;

        var pagedGrades = grades
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedGrades.Select(g => new GradeResponse
        {
            Id = g.Id,
            CourseCode = g.CourseInstance?.Course?.Code ?? null,
            CourseName = g.CourseInstance?.Course?.Name ?? null,
            Section = g.CourseInstance?.Section ?? null,
            Score = g.Score,
            LetterGrade = g.LetterGrade,
            SubmittedAt = g.SubmittedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} grades for student {StudentId}. Page {PageIndex}/{TotalPages}",
            dtos.Count, studentId, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<GradeResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {dtos.Count} grades"
        );
    }
    public async Task<ResultService<GradeResponse>> GetStudentCourseGradeAsync(int studentId, int courseInstanceId)
    {
        var grade = await _unitOfWork.Grades.GetStudentCourseGradeAsync(studentId, courseInstanceId);

        if (grade == null)
        {
            _logger.LogWarning("Grade not found for student {StudentId} in course {CourseInstanceId}",
                studentId, courseInstanceId);
            return ResultService<GradeResponse>.NotFound("Grade not found");
        }

        var dto = new GradeResponse
        {
            Id = grade.Id,
            CourseCode = grade.CourseInstance?.Course?.Code ?? null,
            CourseName = grade.CourseInstance?.Course?.Name ?? null,
            Section = grade.CourseInstance?.Section ?? null,
            Score = grade.Score,
            LetterGrade = grade.LetterGrade,
            SubmittedAt = grade.SubmittedAt
        };

        return ResultService<GradeResponse>.Ok(dto);
    }
    public async Task<PagedResultService<StudentGradeResponse>> GetCourseGradesAsync(int courseInstanceId, int pageIndex = 1, int pageSize = 10)
    {

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var grades = await _unitOfWork.Grades.GetCourseGradesAsync(courseInstanceId);

        var totalCount = grades.Count;

        var pagedGrades = grades
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedGrades.Select(g => new StudentGradeResponse
        {
            Id = g.Id,
            StudentId = g.StudentId,
            StudentName = $"{g.Student?.FirstName} {g.Student?.LastName}",
            StudentEmail = g.Student?.Email ?? null,
            Score = g.Score,
            LetterGrade = g.LetterGrade,
            SubmittedAt = g.SubmittedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} grades for course. Page {PageIndex}/{TotalPages}",
            dtos.Count, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<StudentGradeResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {dtos.Count} grades"
        );
    }
    public async Task<ResultService<GradeResponse>> AssignGradeAsync(AssignGradeRequest request)
    {
        _logger.LogInformation("Assigning grade to student {StudentId} in course {CourseInstanceId}",
            request?.StudentId, request?.CourseInstanceId);

        if (request == null)
            return ResultService<GradeResponse>.Fail("Grade data required");

        if (request.Score < 0 || request.Score > 100)
        {
            _logger.LogWarning("Grade score {Score} out of range", request.Score);
            return ResultService<GradeResponse>.Fail("Score must be between 0 and 100");
        }

        var student = await _unitOfWork.Users.GetByIdAsync(request.StudentId);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            return ResultService<GradeResponse>.Fail("Student not found");
        }

        var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(request.CourseInstanceId);
        if (courseInstance == null)
        {
            _logger.LogWarning("Course {CourseInstanceId} not found", request.CourseInstanceId);
            return ResultService<GradeResponse>.Fail("Course not found");
        }

        var hasGrade = await _unitOfWork.Grades.StudentHasGradeAsync(request.StudentId, request.CourseInstanceId);
        if (hasGrade)
        {
            _logger.LogWarning("Grade already exists for student {StudentId} in course {CourseInstanceId}",
                request.StudentId, request.CourseInstanceId);
            return ResultService<GradeResponse>.Fail("Grade already assigned", ResultErrorCode.Conflict);
        }

        var grade = new Grade
        {
            StudentId = request.StudentId,
            CourseInstanceId = request.CourseInstanceId,
            SubmittedByTeacherId = courseInstance.TeacherId,
            Score = request.Score,
            LetterGrade = request.LetterGrade,
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Grades.AddAsync(grade);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Grade assigned: student {StudentId} scored {Score}", request.StudentId, request.Score);

        var dto = new GradeResponse
        {
            Id = grade.Id,
            Score = grade.Score,
            LetterGrade = grade.LetterGrade,
            SubmittedAt = grade.SubmittedAt
        };

        return ResultService<GradeResponse>.Ok(dto, "Grade assigned");
    }
    public async Task<ResultService<GradeResponse>> UpdateGradeAsync(int gradeId, int teacherId, UpdateGradeRequest request)
    {

        var grade = await _unitOfWork.Grades.GetByIdAsync(gradeId);
        if (grade == null)
        {
            _logger.LogWarning("Grade {GradeId} not found", gradeId);
            return ResultService<GradeResponse>.NotFound("Grade not found");
        }

        if (request == null)
            return ResultService<GradeResponse>.Fail("Grade data required");

        if (request.Score < 0 || request.Score > 100)
        {
            _logger.LogWarning("Grade score {Score} out of range", request.Score);
            return ResultService<GradeResponse>.Fail("Score must be between 0 and 100");
        }

        if (grade.SubmittedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} tried to modify grade {GradeId} belonging to Teacher {OwnerId}",
            teacherId, gradeId, grade.SubmittedByTeacherId);
            return ResultService<GradeResponse>.Fail("You are not the teacher of this course", ResultErrorCode.Forbidden);
        }
        var oldScore = grade.Score;
        var oldLetterGrade = grade.LetterGrade;

        grade.Score = request.Score;
        grade.LetterGrade = request.LetterGrade;
        grade.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Grades.UpdateAsync(grade);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Grade {GradeId} updated from {OldScore} to {NewScore}",
            gradeId, oldScore, request.Score);

        var dto = new GradeResponse
        {
            Id = grade.Id,
            Score = grade.Score,
            LetterGrade = grade.LetterGrade,
            SubmittedAt = grade.SubmittedAt
        };

        return ResultService<GradeResponse>.Ok(dto, $"Grade updated from {oldLetterGrade} to {grade.LetterGrade}");
    }
    public async Task<ResultService> DeleteGradeAsync(int gradeId, int teacherId)
    {
        var grade = await _unitOfWork.Grades.GetByIdAsync(gradeId);
        if (grade == null)
        {
            _logger.LogWarning("Grade {GradeId} not found", gradeId);
            return ResultService.NotFound("Grade not found");
        }
        if (grade.SubmittedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} tried to delete grade {GradeId} belonging to Teacher {OwnerId}",
            teacherId, gradeId, grade.SubmittedByTeacherId);
            return ResultService<GradeResponse>.Fail("You are not the teacher of this course", ResultErrorCode.Forbidden);
        }
        await _unitOfWork.Grades.DeleteAsync(grade);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Grade {GradeId} deleted", gradeId);
        return ResultService.Ok("Grade deleted");
    }

}