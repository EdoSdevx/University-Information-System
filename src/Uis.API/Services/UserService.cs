using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.User;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultService<UserResponse>> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if(user == null)
        {
            _logger.LogWarning($"User with id: {id} does not exist.");
            return ResultService<UserResponse>.NotFound("User not found");
        }

        var dto = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            DepartmentId = user.DepartmentId ?? 0
        };
        return ResultService<UserResponse>.Ok(dto);
    }

    public async Task<ResultService<UserResponse>> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Email validation failed: empty");
            return ResultService<UserResponse>.Fail("Email required");
        }

        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user == null)
        {
            _logger.LogWarning("User with email {Email} not found", email);
            return ResultService<UserResponse>.NotFound("User not found");
        }

        var dto = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            DepartmentId = user.DepartmentId ?? 0
        };

        return ResultService<UserResponse>.Ok(dto);
    }
    public async Task<ResultService<TeacherResponse>> GetTeacherWithCoursesAsync(int teacherId)
    { 
        var teacher = await _unitOfWork.Users.GetTeacherWithCoursesAsync(teacherId);

        if (teacher == null)
        {
            _logger.LogWarning("Teacher {TeacherId} not found", teacherId);
            return ResultService<TeacherResponse>.NotFound("Teacher not found");
        }

        var courseCount = teacher.TaughtCourses?.Count ?? 0;

        var dto = new TeacherResponse
        {
            Id = teacher.Id,
            Email = teacher.Email,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            CourseCount = courseCount
        };

        _logger.LogInformation("Retrieved teacher {TeacherId} with {CourseCount} courses", teacherId, courseCount);
        return ResultService<TeacherResponse>.Ok(dto);
    }
}