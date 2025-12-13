using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs;
using Uis.API.DTOs.User;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    private readonly UniversitySettings _universitySettings;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IOptions<UniversitySettings> universitySettings)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _universitySettings = universitySettings.Value;

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
    public async Task<ResultService<StudentProfileResponse>> GetStudentProfileAsync(int studentId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (user == null)
        {
            _logger.LogWarning("Student with id: {StudentId} does not exist.", studentId);
            return ResultService<StudentProfileResponse>.NotFound("Student not found");
        }

        var profileDto = new StudentProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            City = user.City,
            Major = user.Major,
            AcademicYear = user.AcademicYear,
            EmergencyContactName = user.EmergencyContactName,
            EmergencyContactPhone = user.EmergencyContactPhone,
            EmergencyContactRelationship = user.EmergencyContactRelationship,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };

        _logger.LogInformation("Retrieved student profile for user {UserId}", studentId);
        return ResultService<StudentProfileResponse>.Ok(profileDto);
    }
    public async Task<ResultService<StudentProfileResponse>> UpdateStudentProfileAsync(int studentId, StudentUpdateProfileRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (user == null)
        {
            _logger.LogWarning("Student with id: {StudentId} does not exist.", studentId);
            return ResultService<StudentProfileResponse>.NotFound("Student not found");
        }

        if (!string.IsNullOrEmpty(request.FirstName))
            user.FirstName = request.FirstName;
        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber;
        if (request.DateOfBirth.HasValue)
            user.DateOfBirth = request.DateOfBirth;
        if (!string.IsNullOrEmpty(request.Address))
            user.Address = request.Address;
        if (!string.IsNullOrEmpty(request.City))
            user.City = request.City;
        if (!string.IsNullOrEmpty(request.EmergencyContactName))
            user.EmergencyContactName = request.EmergencyContactName;
        if (!string.IsNullOrEmpty(request.EmergencyContactPhone))
            user.EmergencyContactPhone = request.EmergencyContactPhone;
        if (!string.IsNullOrEmpty(request.EmergencyContactRelationship))
            user.EmergencyContactRelationship = request.EmergencyContactRelationship;

        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var profileDto = new StudentProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            City = user.City,
            Major = user.Major,
            AcademicYear = user.AcademicYear,
            EmergencyContactName = user.EmergencyContactName,
            EmergencyContactPhone = user.EmergencyContactPhone,
            EmergencyContactRelationship = user.EmergencyContactRelationship,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };

        _logger.LogInformation("Updated profile for student {StudentId}", studentId);
        return ResultService<StudentProfileResponse>.Ok(profileDto);
    }

    public async Task<ResultService<bool>> ChangePasswordAsync(int userId, ChangePasswordRequest model)
    {
        if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
        {
            _logger.LogWarning("Password change failed: empty passwords for user {UserId}", userId);
            return ResultService<bool>.Fail("Passwords cannot be empty");
        }

        if (model.NewPassword.Length < 8)
        {
            _logger.LogWarning("Password change failed: password too short for user {UserId}", userId);
            return ResultService<bool>.Fail("New password must be at least 8 characters long");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Password change failed: user {UserId} not found", userId);
            return ResultService<bool>.NotFound("User not found");
        }
        if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
        {
            _logger.LogWarning("Password change failed: incorrect current password for user {UserId}", userId);
            return ResultService<bool>.Fail("Current password is incorrect");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Password changed successfully for user {UserId}", userId);
        return ResultService<bool>.Ok(true);
    }
    public async Task<ResultService<HelpContactsResponse>> GetHelpContactsAsync(int studentId)
    {
        _logger.LogInformation("Fetching help contacts for student {StudentId}", studentId);

        var student = await _unitOfWork.Users.GetByIdAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", studentId);
            return ResultService<HelpContactsResponse>.NotFound("Student not found");
        }

   
        var department = await _unitOfWork.Departments.GetByIdAsync(student.DepartmentId.Value);

        var instructors = await _unitOfWork.Enrollments.GetStudentInstructorsAsync(studentId);

        var response = new HelpContactsResponse
        {
            DepartmentName = department?.Name,
            DepartmentEmail = department?.Email,
            DepartmentSecretaryEmail = department?.SecretaryEmail,
            StudentAffairsEmail = _universitySettings.StudentAffairsEmail,
            RegistrarEmail = _universitySettings.RegistrarEmail,
            ITSupportEmail = _universitySettings.ITSupportEmail,
            CourseInstructors = instructors
        };

        _logger.LogInformation("Retrieved help contacts for student {StudentId}: {InstructorCount} instructors",
            studentId, instructors.Count);

        return ResultService<HelpContactsResponse>.Ok(response);
    }
}