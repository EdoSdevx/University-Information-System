using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Enrollment;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(IUnitOfWork unitOfWork, ILogger<EnrollmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResultService<EnrollmentResponse>> GetStudentEnrollmentsAsync(int studentId, int pageIndex = 1, int pageSize = 10)
    {

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var student = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", studentId);
            return PagedResultService<EnrollmentResponse>.Fail("Student not found");
        }

        var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsAsync(studentId);

        var totalCount = enrollments.Count;

        var pagedEnrollments = enrollments
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedEnrollments.Select(e => new EnrollmentResponse
        {
            Id = e.Id,
            CourseInstanceId = e.CourseInstanceId,
            CourseCode = e.CourseInstance?.Course?.Code ?? null,
            CourseName = e.CourseInstance?.Course?.Name ?? null,
            Section = e.CourseInstance?.Section ?? null,
            TeacherName = $"{e.CourseInstance?.Teacher?.FirstName} {e.CourseInstance?.Teacher?.LastName}",
            AcademicYear = e.CourseInstance?.AcademicYear?.Year ?? null,
            Status = e.Status.ToString(),
            EnrolledAt = e.EnrolledAt,
            DroppedAt = e.DroppedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} enrollments for student {StudentId}. Page {PageIndex}/{TotalPages}",
            dtos.Count, studentId, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<EnrollmentResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {dtos.Count} enrollments"
        );
    }
    public async Task<ResultService<EnrollmentResponse>> EnrollStudentAsync(int studentId, EnrollStudentRequest request)
    {
        return await ExecuteEnrollmentAsync(studentId, request.CourseInstanceId, false);
    }

    public async Task<ResultService<EnrollmentResponse>> AdminEnrollStudentAsync(AdminEnrollStudentRequest request)
    {
        return await ExecuteEnrollmentAsync(request.StudentId, request.CourseInstanceId, true);
    }

    public async Task<ResultService> AdminDropCourseAsync(AdminDropCourseRequest request)
    {
        return await ExecuteDropCourse(request.StudentId, request.CourseInstanceId);
    }
    public async Task<ResultService> DropCourseAsync(int studentId, DropCourseRequest request)
    {
        return await ExecuteDropCourse(studentId, request.CourseInstanceId);
    }

    public async Task<PagedResultService<StudentEnrollmentResponse>> GetCourseEnrollmentsAsync(int courseInstanceId, int pageIndex = 1, int pageSize = 10, int teacherId = -1, bool byPass = false)
    {
        _logger.LogInformation("Retrieving enrollments for course {CourseInstanceId}. Page {PageIndex}, Size {PageSize}",
            courseInstanceId, pageIndex, pageSize);

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        if(!byPass && teacherId == -1)
        {
            return PagedResultService<StudentEnrollmentResponse>.Fail($"Wrong teacherId: {teacherId}.");
        }
        var courseInstance = await _unitOfWork.CourseInstances.FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);

        if(courseInstance == null)
        {
            return PagedResultService<StudentEnrollmentResponse>.Fail($"No such course instance exist with id: {courseInstanceId}.");
        }

        if(!byPass)
        {
            if (courseInstance.TeacherId != teacherId)
            {
                return PagedResultService<StudentEnrollmentResponse>.Fail("You are not authorized to see other teachers courses.");
            }
        }
        var enrollments = await _unitOfWork.Enrollments.GetCourseEnrollmentsAsync(courseInstanceId);

        var totalCount = enrollments.Count;

        var pagedEnrollments = enrollments
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedEnrollments.Select(e => new StudentEnrollmentResponse
        {
            Id = e.Id,
            EnrollmentId = e.Id,
            StudentId = e.StudentId,
            StudentName = $"{e.Student?.FirstName} {e.Student?.LastName}",
            StudentEmail = e.Student?.Email ?? null,
            EnrolledAt = e.EnrolledAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} enrollments for course. Page {PageIndex}/{TotalPages}",
            dtos.Count, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<StudentEnrollmentResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {dtos.Count} enrollments"
        );
    }
    public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseInstanceId)
    {
        return await _unitOfWork.Enrollments.IsStudentEnrolledAsync(studentId, courseInstanceId);
    }

    private async Task<ResultService<EnrollmentResponse>> ExecuteEnrollmentAsync(int studentId, int courseInstanceId, bool ignoreCapacity = false)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var courseInstance = await _unitOfWork.CourseInstances.GetByIdWithLockAsync(courseInstanceId);

            if (courseInstance == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResultService<EnrollmentResponse>.Fail("Course not found");
            }

            var isEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(studentId, courseInstanceId);
            if (isEnrolled)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResultService<EnrollmentResponse>.Fail("Already enrolled", ResultErrorCode.Conflict);
            }

            if (!ignoreCapacity && courseInstance.CurrentEnrollmentCount >= courseInstance.Capacity)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ResultService<EnrollmentResponse>.Fail("Course is full", ResultErrorCode.CourseFull);
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseInstanceId = courseInstanceId,
                AcademicYearId = courseInstance.AcademicYearId,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);

            courseInstance.CurrentEnrollmentCount++;
            await _unitOfWork.CourseInstances.UpdateAsync(courseInstance);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Success: Student {StudentId} -> Course {CourseId}", studentId, courseInstanceId);

            return ResultService<EnrollmentResponse>.Ok(new EnrollmentResponse
            {
                Id = enrollment.Id,
            }, "Enrolled successfully");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Enrollment error");
            return ResultService<EnrollmentResponse>.Fail("Enrollment failed: " + ex.Message);
        }
    }
    private async Task<ResultService> ExecuteDropCourse(int studentId, int courseInstanceId)
    {
        _logger.LogInformation("Dropping course for student {StudentId} from course {CourseInstanceId}",
            studentId, courseInstanceId);

        var enrollment = await _unitOfWork.Enrollments.GetEnrollmentAsync(
            studentId,
            courseInstanceId);

        if (enrollment == null)
        {
            _logger.LogWarning("Enrollment not found for student {StudentId} in course {CourseInstanceId}",
                studentId, courseInstanceId);
            return ResultService.NotFound("Not enrolled in this course");
        }

        if (enrollment.Status == EnrollmentStatus.Dropped)
        {
            _logger.LogWarning("Already dropped for student {StudentId}", studentId);
            return ResultService.Fail("Already dropped");
        }

        try
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Enrollments.DeleteAsync(enrollment);

                var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(courseInstanceId);
                if (courseInstance != null && courseInstance.CurrentEnrollmentCount > 0)
                {
                    courseInstance.CurrentEnrollmentCount--;
                    await _unitOfWork.CourseInstances.UpdateAsync(courseInstance);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Course dropped for student {StudentId}", studentId);
                return ResultService.Ok("Course dropped successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Drop transaction failed");
                throw;  
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dropping course: {Message}", ex.Message);
            return ResultService.Fail("Drop failed. Please try again.");
        }
    }
}