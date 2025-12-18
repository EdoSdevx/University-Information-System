using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Course;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CourseService> _logger;

    public CourseService(IUnitOfWork unitOfWork, ILogger<CourseService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResultService<CourseDetailResponse>> GetAllCoursesAsync(int pageIndex = 1, int pageSize = 10, string? searchTerm = null)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        _logger.LogInformation("Retrieving all courses. Page {PageIndex}, Size {PageSize}", pageIndex, pageSize);

        var allCourses = await _unitOfWork.Courses.GetAllAsync();

        IEnumerable<Course> filteredQuery = allCourses;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLowerInvariant();

            filteredQuery = filteredQuery.Where(c =>
                (c.Code != null && c.Code.ToLowerInvariant().Contains(searchTerm)) ||
                (c.Name != null && c.Name.ToLowerInvariant().Contains(searchTerm))
            );
        }

        var totalFilteredCount = filteredQuery.Count();

        var pagedCourses = filteredQuery
                                .OrderBy(c => c.Name)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

        var dtos = pagedCourses.Select(course => new CourseDetailResponse
        {
            Id = course.Id,
            CreditHours = course.CreditHours,
            PrerequisiteCourseId = course.PrerequisiteCourseId,
            Name = course.Name,
            Code = course.Code,
            DepartmentId = course.DepartmentId
        }).ToList();

        return PagedResultService<CourseDetailResponse>.Ok(dtos, pageIndex, pageSize, totalFilteredCount);
    }
    public async Task<ResultService<CourseResponse>> GetCourseByCodeAsync(string code)
    {

        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogWarning("Course code validation failed: code is null or empty");
            return ResultService<CourseResponse>.Fail("Course code cannot be empty");
        }

        var course = await _unitOfWork.Courses.GetByCodeAsync(code);

        if (course == null)
        {
            _logger.LogWarning("Course with code {Code} not found", code);
            return ResultService<CourseResponse>.NotFound("Course not found");
        }

        var dto = new CourseResponse
        {
            Code = course.Code,
            Name = course.Name,
            CreditHours = course.CreditHours,
            DepartmentId = course.DepartmentId
        };

        return ResultService<CourseResponse>.Ok(dto, "Course retrieved");
    }

    public async Task<PagedResultService<CourseResponse>> GetByDepartmentAsync(int departmentId, int pageIndex = 1, int pageSize = 10)
    {

        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var courses = await _unitOfWork.Courses.GetByDepartmentAsync(departmentId);

        var totalCount = courses.Count;

        var pagedCourses = courses
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = pagedCourses.Select(c => new CourseResponse
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            CreditHours = c.CreditHours,
            DepartmentId = c.DepartmentId,
        }).ToList();

        _logger.LogInformation("Retrieved {Count} courses for department. Page {PageIndex}/{TotalPages}",
            dtos.Count, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<CourseResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {dtos.Count} courses"
        );
    }

    public async Task<ResultService<CourseDetailResponse>> CreateCourseAsync(CreateCourseRequest request)
    {
        var existingCourse = await _unitOfWork.Courses.GetByCodeAsync(request.Code!);

        if (existingCourse != null)
            return ResultService<CourseDetailResponse>.Fail("Course code already exists");

        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        if (department == null)
            return ResultService<CourseDetailResponse>.Fail("Department not found");

        if (request.PrerequisiteCourseId.HasValue)
        {
            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.PrerequisiteCourseId.Value);
            if (prerequisite == null)
                return ResultService<CourseDetailResponse>.Fail("Prerequisite course not found");
        }

        var course = new Course
        {
            Code = request.Code!,
            Name = request.Name!,
            CreditHours = request.CreditHours,
            DepartmentId = request.DepartmentId,
            PrerequisiteCourseId = request.PrerequisiteCourseId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        var response = new CourseDetailResponse
        {
            Id = course.Id,
            Code = course.Code,
            Name = course.Name,
            CreditHours = course.CreditHours,
            DepartmentId = course.DepartmentId,
            DepartmentName = department.Name,
            PrerequisiteCourseId = course.PrerequisiteCourseId
        };

        return ResultService<CourseDetailResponse>.Ok(response, "Course created successfully");
    }
    public async Task<ResultService<CourseResponse>> UpdateCourseAsync(int id, UpdateCourseRequest request)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return ResultService<CourseResponse>.NotFound($"Course with ID {id} not found");

        if (request.Code != course.Code)
        {
            var existingCourse = await _unitOfWork.Courses.GetByCodeAsync(request.Code!);
            if (existingCourse != null)
                return ResultService<CourseResponse>.Fail("Course code already exists");
        }

        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        if (department == null)
            return ResultService<CourseResponse>.Fail("Department not found");

        if (request.PrerequisiteCourseId.HasValue)
        {
            if (request.PrerequisiteCourseId.Value == id)
                return ResultService<CourseResponse>.Fail("A course cannot be its own prerequisite");

            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.PrerequisiteCourseId.Value);
            if (prerequisite == null)
                return ResultService<CourseResponse>.Fail("Prerequisite course not found");
        }

        course.Code = request.Code!;
        course.Name = request.Name!;
        course.CreditHours = request.CreditHours;
        course.DepartmentId = request.DepartmentId;
        course.PrerequisiteCourseId = request.PrerequisiteCourseId;
        course.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        var response = new CourseResponse
        {
            Id = course.Id,
            Code = course.Code,
            Name = course.Name,
            CreditHours = course.CreditHours,
            DepartmentId = course.DepartmentId,
            DepartmentCode = department.Code,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };

        return ResultService<CourseResponse>.Ok(response, "Course updated successfully");
    }

    public async Task<ResultService> DeleteCourseAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return ResultService.NotFound($"Course with ID {id} not found");

        var hasInstances = await _unitOfWork.CourseInstances.AnyAsync(ci => ci.CourseId == id);

        if (hasInstances)
            return ResultService.Fail("Cannot delete course with existing instances");

        await _unitOfWork.Courses.DeleteAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ResultService.Ok("Course deleted successfully");
    }
}

