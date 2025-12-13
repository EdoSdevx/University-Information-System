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
        _logger.LogInformation("Enrolling student {StudentId} in course instance {CourseInstanceId}",
           studentId, request?.CourseInstanceId);

        if (request == null)
            return ResultService<EnrollmentResponse>.Fail("Enrollment data required");

        var student = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", studentId);
            return ResultService<EnrollmentResponse>.Fail("Student not found");
        }

        var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(request.CourseInstanceId);
        if (courseInstance == null)
        {
            _logger.LogWarning("Course instance {CourseInstanceId} not found", request.CourseInstanceId);
            return ResultService<EnrollmentResponse>.Fail("Course not found");
        }

        var isEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(
            studentId,
            request.CourseInstanceId);
        if (isEnrolled)
        {
            _logger.LogWarning("Student {StudentId} already enrolled in course {CourseInstanceId}",
                studentId, request.CourseInstanceId);
            return ResultService<EnrollmentResponse>.Fail("Already enrolled in this course", ResultErrorCode.Conflict);
        }

        var hasCapacity = await _unitOfWork.CourseInstances.HasCapacityAsync(request.CourseInstanceId);
        if (!hasCapacity)
        {
            _logger.LogWarning("Course {CourseInstanceId} is full", request.CourseInstanceId);
            return ResultService<EnrollmentResponse>.Fail("Course is full", ResultErrorCode.CourseFull);
        }

        try
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseInstanceId = request.CourseInstanceId,
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

                _logger.LogInformation("Student {StudentId} enrolled in course {CourseInstanceId}",
                    studentId, request.CourseInstanceId);

                var dto = new EnrollmentResponse
                {
                    Id = enrollment.Id,
                    CourseCode = courseInstance.Course?.Code ?? null,
                    CourseName = courseInstance.Course?.Name ?? null,
                    Section = courseInstance.Section,
                    Status = enrollment.Status.ToString(),
                    EnrolledAt = enrollment.EnrolledAt
                };

                return ResultService<EnrollmentResponse>.Ok(dto, "Enrolled successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Enrollment transaction failed");
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enrolling student: {Message}", ex.Message);
            return ResultService<EnrollmentResponse>.Fail("Enrollment failed. Please try again.");
        }
    }
    public async Task<ResultService> DropCourseAsync(int studentId, DropCourseRequest request)
    {
        _logger.LogInformation("Dropping course for student {StudentId} from course {CourseInstanceId}",
            studentId, request?.CourseInstanceId);

        if (request == null)
            return ResultService.Fail("Drop request required");

        var enrollment = await _unitOfWork.Enrollments.GetEnrollmentAsync(
            studentId,
            request.CourseInstanceId);

        if (enrollment == null)
        {
            _logger.LogWarning("Enrollment not found for student {StudentId} in course {CourseInstanceId}",
                studentId, request.CourseInstanceId);
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

                var courseInstance = await _unitOfWork.CourseInstances.GetByIdAsync(request.CourseInstanceId);
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
    public async Task<PagedResultService<StudentEnrollmentResponse>> GetCourseEnrollmentsAsync(int courseInstanceId, int pageIndex = 1, int pageSize = 10)
    {
        _logger.LogInformation("Retrieving enrollments for course {CourseInstanceId}. Page {PageIndex}, Size {PageSize}",
            courseInstanceId, pageIndex, pageSize);

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

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

}