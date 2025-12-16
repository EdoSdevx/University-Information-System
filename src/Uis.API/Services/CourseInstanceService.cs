using Azure.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Course;
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
            Day1 = (int)ci.Day1,
            Day2 = (int)ci.Day2,
            StartTime = ci.StartTime?.ToString("HH:mm"),
            EndTime = ci.EndTime?.ToString("HH:mm"),
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
            Day1 = (int)ci.Day1,
            Day2 = (int)ci.Day2,
            StartTime = ci.StartTime?.ToString("HH:mm"),
            EndTime = ci.EndTime?.ToString("HH:mm"),
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
            Day1 = (int)course.Day1,
            Day2 = (int)course.Day2,
            StartTime = course.StartTime?.ToString("HH:mm"),
            EndTime = course.EndTime?.ToString("HH:mm"),
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
            Day1 = (int)ci.Day1,
            Day2 = (int)ci.Day2,
            StartTime = ci.StartTime?.ToString("HH:mm"),
            EndTime = ci.EndTime?.ToString("HH:mm"),
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
    public async Task<PagedResultService<CourseInstanceResponse>> GetAllInstancesAsync(int? academicYearId, int? departmentId, string? searchTerm, int pageIndex, int pageSize)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var instances = await _unitOfWork.CourseInstances.GetAllWithDetailsAsync();

        if (academicYearId.HasValue)
        {
            instances = instances.Where(i => i.AcademicYearId == academicYearId.Value).ToList();
        }

        if (departmentId.HasValue)
        {
            instances = instances.Where(i => i.DepartmentId == departmentId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            instances = instances.Where(i =>
                (i.Course != null && i.Course.Code != null && i.Course.Code.ToLower().Contains(term)) ||
                (i.Course != null && i.Course.Name != null && i.Course.Name.ToLower().Contains(term))
            ).ToList();
        }

        var totalCount = instances.Count;

        var pagedInstances = instances
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedInstances.Select(i => new CourseInstanceResponse
        {
            CourseInstanceId = i.Id,
            CourseCode = i.Course?.Code,
            CourseName = i.Course?.Name,
            Section = i.Section,
            TeacherName = i.Teacher != null ? $"{i.Teacher.FirstName} {i.Teacher.LastName}" : null,
            Day1 = (int?)i.Day1,
            Day2 = (int?)i.Day2,
            StartTime = i.StartTime?.ToString("HH:mm"),
            EndTime = i.EndTime?.ToString("HH:mm"),
            Location = i.Location,
            Capacity = i.Capacity,
            CurrentEnrollmentCount = i.Enrollments?.Count(e => e.Status == EnrollmentStatus.Active) ?? 0,
            AcademicYear = i.AcademicYear?.Year,
            DepartmentName = i.Department?.Name
        }).ToList();

        _logger.LogInformation("Retrieved {Count} course instances. Page {PageIndex}/{TotalPages}",
            dtos.Count, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<CourseInstanceResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} course instances"
        );
    }
    public async Task<ResultService<CourseInstanceDetailResponse>> GetByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to get course instance details for course {CourseId}", id);

        var instance = await _unitOfWork.CourseInstances.GetByIdAsync(id);

        if(instance == null)
        {
            _logger.LogWarning("Course instance could not found : course {CourseId} not found", id);
            return ResultService<CourseInstanceDetailResponse>.NotFound("Course not found");
        }

        var dto = new CourseInstanceDetailResponse
        {
            CourseId = instance.CourseId,
            TeacherId = instance.TeacherId,
            AcademicYearId = instance.AcademicYearId,
            DepartmentId = instance.DepartmentId,
            Section = instance.Section,
            Capacity = instance.Capacity,
            Day1 = instance.Day1.HasValue ? (int)instance.Day1.Value : null,
            Day2 = instance.Day2.HasValue ? (int)instance.Day2.Value : null,
            StartTime = instance.StartTime?.ToString("HH:mm"),
            EndTime = instance.EndTime?.ToString("HH:mm"),
            Location = instance.Location,
            CreatedAt = DateTime.UtcNow
        };

        return ResultService<CourseInstanceDetailResponse>.Ok(dto);
    }
    public async Task<ResultService<CourseInstanceResponse>> CreateInstanceAsync(CreateCourseInstanceRequest request)

    {
        _logger.LogInformation("Attempting to create new course instance for course {CourseId}", request.CourseId);
        var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
        if (course == null)
        {
            _logger.LogWarning("Course instance creation failed: course {CourseId} not found", request.CourseId);
            return ResultService<CourseInstanceResponse>.NotFound("Course not found");
        }
        var teacher = await _unitOfWork.Users.GetByIdAsync(request.TeacherId);
        if (teacher == null || teacher.Role != UserRole.Teacher)
        {
            _logger.LogWarning("Course instance creation failed: teacher {TeacherId} not found", request.TeacherId);
            return ResultService<CourseInstanceResponse>.NotFound("Teacher not found");
        }
        var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(request.AcademicYearId);
        if (academicYear == null)
        {
            _logger.LogWarning("Course instance creation failed: academic year {AcademicYearId} not found", request.AcademicYearId);
            return ResultService<CourseInstanceResponse>.NotFound("Academic year not found");

        }

        var instance = new CourseInstance
        {
            CourseId = request.CourseId,
            TeacherId = request.TeacherId,
            AcademicYearId = request.AcademicYearId,
            DepartmentId = course.DepartmentId,
            Section = request.Section,
            Capacity = request.Capacity,
            Day1 = request.Day1.HasValue ? (DayOfWeek)request.Day1.Value : null,
            Day2 = request.Day2.HasValue ? (DayOfWeek)request.Day2.Value : null,
            StartTime = TimeOnly.Parse(request.StartTime),
            EndTime = TimeOnly.Parse(request.EndTime),
            Location = request.Location,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.CourseInstances.AddAsync(instance);
        await _unitOfWork.SaveChangesAsync();

        var response = new CourseInstanceResponse
        {
            CourseInstanceId = instance.Id,
            CourseCode = course.Code,
            CourseName = course.Name,
            Section = instance.Section,
            TeacherName = $"{teacher.FirstName} {teacher.LastName}",
            Day1 = (int)instance.Day1,
            Day2 = request.Day2.HasValue ? (int)request.Day2.Value : null,
            StartTime = instance.StartTime?.ToString("HH:mm"),
            EndTime = instance.EndTime?.ToString("HH:mm"),
            Location = instance.Location,
            Capacity = instance.Capacity,
            CurrentEnrollmentCount = 0,
            AcademicYear = academicYear.Year,
            DepartmentName = course.Department?.Name
        };
        _logger.LogInformation("Course instance created successfully with ID: {InstanceId}", instance.Id);
        return ResultService<CourseInstanceResponse>.Ok(response, "Course instance created successfully");
    }

    public async Task<ResultService<CourseInstanceResponse>> UpdateInstanceAsync(int instanceId, UpdateCourseInstanceRequest request)
    {
        _logger.LogInformation("Attempting to update course instance with ID: {InstanceId}", instanceId);

        var instance = await _unitOfWork.CourseInstances.GetByIdWithDetailsAsync(instanceId);
        if (instance == null)
        {
            _logger.LogWarning("Course instance update failed: instance {InstanceId} not found", instanceId);
            return ResultService<CourseInstanceResponse>.NotFound("Course instance not found");
        }

        if (request.TeacherId.HasValue)
        {
            var teacher = await _unitOfWork.Users.GetByIdAsync(request.TeacherId.Value);
            if (teacher == null || teacher.Role != UserRole.Teacher)
            {
                _logger.LogWarning("Course instance update failed: teacher {TeacherId} not found", request.TeacherId);
                return ResultService<CourseInstanceResponse>.NotFound("Teacher not found");
            }
            instance.TeacherId = request.TeacherId.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Section))
            instance.Section = request.Section;

        instance.Capacity = request.Capacity;

        if (request.Day1.HasValue)
            instance.Day1 = (DayOfWeek)request.Day1.Value;

        if (request.Day2.HasValue)
            instance.Day2 = (DayOfWeek)request.Day2.Value;

        if (!string.IsNullOrWhiteSpace(request.StartTime))
            instance.StartTime = TimeOnly.Parse(request.StartTime);

        if (!string.IsNullOrWhiteSpace(request.EndTime))
            instance.EndTime = TimeOnly.Parse(request.EndTime);

        if (request.Location != null)
            instance.Location = request.Location;

        instance.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CourseInstances.UpdateAsync(instance);
        await _unitOfWork.SaveChangesAsync();

        var updatedInstance = await _unitOfWork.CourseInstances.GetByIdWithDetailsAsync(instanceId);

        var response = new CourseInstanceResponse
        {
            CourseInstanceId = updatedInstance!.Id,
            CourseCode = updatedInstance.Course?.Code,
            CourseName = updatedInstance.Course?.Name,
            Section = updatedInstance.Section,
            TeacherName = updatedInstance.Teacher != null ? $"{updatedInstance.Teacher.FirstName} {updatedInstance.Teacher.LastName}" : null,
            Day1 = (int)updatedInstance.Day1,
            Day2 = (int)updatedInstance.Day2,
            StartTime = updatedInstance.StartTime?.ToString("HH:mm"),
            EndTime = updatedInstance.EndTime?.ToString("HH:mm"),
            Location = updatedInstance.Location,
            Capacity = updatedInstance.Capacity,
            CurrentEnrollmentCount = updatedInstance.Enrollments?.Count(e => e.Status == EnrollmentStatus.Active) ?? 0,
            AcademicYear = updatedInstance.AcademicYear?.Year,
            DepartmentName = updatedInstance.Department?.Name
        };

        _logger.LogInformation("Course instance updated successfully: {InstanceId}", instanceId);
        return ResultService<CourseInstanceResponse>.Ok(response, "Course instance updated successfully");
    }

    public async Task<ResultService> DeleteInstanceAsync(int instanceId)
    {
        _logger.LogInformation("Attempting to delete course instance with ID: {InstanceId}", instanceId);

        var instance = await _unitOfWork.CourseInstances.GetByIdAsync(instanceId);
        if (instance == null)
        {
            _logger.LogWarning("Course instance deletion failed: instance {InstanceId} not found", instanceId);
            return ResultService.NotFound("Course instance not found");
        }

        await _unitOfWork.CourseInstances.DeleteAsync(instanceId);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Course instance deleted successfully: {InstanceId}", instanceId);
        return ResultService.Ok("Course instance deleted successfully");
    }
}

    
 