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
}

