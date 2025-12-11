using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.CourseInstance;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class CourseInstanceService : ICourseInstanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CourseInstanceService> _logger;

    public CourseInstanceService(IUnitOfWork unitOfWork, ILogger<CourseInstanceService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ResultService<List<CourseInstanceSchedule>>> GetStudentScheduleAsync(int studentId)
    {
        var courseInstances = await _unitOfWork.CourseInstances.GetStudentScheduleAsync(studentId);

        var schedule = courseInstances.Select(ci => new CourseInstanceSchedule
        {
            CourseInstanceId = ci.Id,
            CourseCode = ci.Course.Code,
            CourseName = ci.Course.Name,
            Section = ci.Section,
            Day1 = ci.Day1,
            Day2 = ci.Day2,
            StartTime = ci.StartTime,
            EndTime = ci.EndTime,
            TeacherName = $"{ci.Teacher.FirstName} {ci.Teacher.LastName}",
            Location = ci.Location
        }).ToList();

        return ResultService<List<CourseInstanceSchedule>>.Ok(schedule);
    }
    public async Task<PagedResultService<CourseInstanceResponse>> GetAvailableCoursesAsync(int studentId, int academicYearId, int pageIndex = 1,int pageSize = 10)
    {

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var student = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (student == null || student.DepartmentId == null)
        {
            _logger.LogWarning("Student {StudentId} not found or not assigned to department", studentId);
            return PagedResultService<CourseInstanceResponse>.Fail("Student not found or not assigned to department");
        }

        var courses = await _unitOfWork.CourseInstances.GetAvailableCoursesAsync(academicYearId);

        var departmentCourses = courses
                               .Where(c => c.DepartmentId == student.DepartmentId)
                               .ToList();

        var totalCount = departmentCourses.Count;

        var pagedCourses = departmentCourses
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedCourses.Select(ci => new CourseInstanceResponse
        {
            CourseCode = ci.Course?.Code ?? null,
            CourseName = ci.Course?.Name ?? null,
            TeacherName = $"{ci.Teacher?.FirstName} {ci.Teacher?.LastName}",
            Credits = ci.Course?.CreditHours ?? 0,
            CourseInstanceId = ci.Id,
            Section = ci.Section,
            Capacity = ci.Capacity,
            CurrentEnrollmentCount = ci.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
            Day1 = ci.Day1,
            Day2 = ci.Day2,
            StartTime = ci.StartTime,
            EndTime = ci.EndTime,
            Location = ci.Location
        }).ToList();

        _logger.LogInformation("Retrieved {Count} available courses for year {YearId}. Page {PageIndex}/{TotalPages}",
            dtos.Count, academicYearId, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<CourseInstanceResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} available courses"
        );
    }

    public async Task<ResultService<CourseInstanceResponse>> GetForEnrollmentAsync(int courseInstanceId)
    {
        var course = await _unitOfWork.CourseInstances.GetForEnrollmentAsync(courseInstanceId);

        if (course == null)
        {
            _logger.LogWarning("Course instance {CourseInstanceId} not found", courseInstanceId);
            return ResultService<CourseInstanceResponse>.NotFound("Course not found");
        }

        var enrollmentCount = course.Enrollments?.Count(e => e.Status == EnrollmentStatus.Active) ?? 0;

        var dto = new CourseInstanceResponse
        {
            CourseCode = course.Course?.Code ?? null,
            CourseName = course.Course?.Name ?? null,
            Credits = course.Course?.CreditHours ?? 0,
            TeacherName = $"{course.Teacher?.FirstName} {course.Teacher?.LastName}",
            Section = course.Section,
            Capacity = course.Capacity,
            CurrentEnrollmentCount = enrollmentCount,
            Day1 = course.Day1,
            Day2 = course.Day2,
            StartTime = course.StartTime,
            EndTime = course.EndTime,
            Location = course.Location,
            AcademicYear = course.AcademicYear?.Year ?? null
        };

        return ResultService<CourseInstanceResponse>.Ok(dto);
    }

    public async Task<ResultService<bool>> HasCapacityAsync(int courseInstanceId)
    {
        var hasCapacity = await _unitOfWork.CourseInstances.HasCapacityAsync(courseInstanceId);

        return ResultService<bool>.Ok(hasCapacity, hasCapacity ? "Course has available seats" : "Course is full");
    }
    public async Task<PagedResultService<CourseInstanceResponse>> GetTeacherCoursesAsync( int teacherId, int pageIndex = 1, int pageSize = 10)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var courses = await _unitOfWork.CourseInstances.GetTeacherCoursesAsync(teacherId);

        var totalCount = courses.Count;

        var pagedCourses = courses
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedCourses.Select(ci => new CourseInstanceResponse
        {
            CourseCode = ci.Course?.Code ?? null,
            CourseName = ci.Course?.Name ?? null,
            Credits = ci.Course?.CreditHours ?? 0,
            Section = ci.Section,
            CourseInstanceId = ci.CourseId,
            Capacity = ci.Capacity,
            CurrentEnrollmentCount = ci.Enrollments?.Count(e => e.Status == EnrollmentStatus.Active) ?? 0,
            AcademicYear = ci.AcademicYear?.Year ?? null,
            StartTime = ci.StartTime,
            EndTime = ci.EndTime,
            Location = ci.Location
        }).ToList();

        _logger.LogInformation("Retrieved {Count} courses for teacher {TeacherId}. Page {PageIndex}/{TotalPages}",
            dtos.Count, teacherId, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<CourseInstanceResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} courses"
        );
    }
}

    
 