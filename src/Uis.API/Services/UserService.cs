using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs;
using Uis.API.DTOs.Authentication;
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
            DepartmentId = user.DepartmentId
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
            DepartmentId = user.DepartmentId
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

   
        var department = await _unitOfWork.Departments.GetByIdAsync(student.DepartmentId);

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

    public async Task<ResultService<TeacherProfileResponse>> GetTeacherProfileAsync(int teacherId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(teacherId);
        if (user == null)
        {
            _logger.LogWarning("Teacher with id: {TeacherId} does not exist.", teacherId);
            return ResultService<TeacherProfileResponse>.NotFound("Teacher not found");
        }

         var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);

        var profileDto = new TeacherProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            City = user.City,
            OfficeLocation = user.OfficeLocation,
            EmergencyContactName = user.EmergencyContactName,
            EmergencyContactPhone = user.EmergencyContactPhone,
            EmergencyContactRelationship = user.EmergencyContactRelationship,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            DepartmentName = department?.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };

        _logger.LogInformation("Retrieved teacher profile for user {UserId}", teacherId);
        return ResultService<TeacherProfileResponse>.Ok(profileDto);
    }

    public async Task<ResultService<TeacherProfileResponse>> UpdateTeacherProfileAsync(int teacherId, TeacherUpdateProfileRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(teacherId);

        if (user == null)
        {
            _logger.LogWarning("Teacher with id: {TeacherId} does not exist.", teacherId);
            return ResultService<TeacherProfileResponse>.NotFound("Teacher not found");
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
        if (!string.IsNullOrEmpty(request.OfficeLocation))
            user.OfficeLocation = request.OfficeLocation;
        if (!string.IsNullOrEmpty(request.EmergencyContactName))
            user.EmergencyContactName = request.EmergencyContactName;
        if (!string.IsNullOrEmpty(request.EmergencyContactPhone))
            user.EmergencyContactPhone = request.EmergencyContactPhone;
        if (!string.IsNullOrEmpty(request.EmergencyContactRelationship))
            user.EmergencyContactRelationship = request.EmergencyContactRelationship;

        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();


        var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);


        var profileDto = new TeacherProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            City = user.City,
            OfficeLocation = user.OfficeLocation,
            EmergencyContactName = user.EmergencyContactName,
            EmergencyContactPhone = user.EmergencyContactPhone,
            EmergencyContactRelationship = user.EmergencyContactRelationship,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            DepartmentName = department?.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };

        _logger.LogInformation("Updated profile for teacher {TeacherId}", teacherId);
        return ResultService<TeacherProfileResponse>.Ok(profileDto);
    }
    public async Task<ResultService<TeacherHelpContactsResponse>> GetTeacherHelpContactsAsync(int teacherId)
    {
        _logger.LogInformation("Fetching help contacts for teacher {TeacherId}", teacherId);

        var teacher = await _unitOfWork.Users.GetByIdAsync(teacherId);

        if (teacher == null)
        {
            _logger.LogWarning("Teacher {TeacherId} not found", teacherId);
            return ResultService<TeacherHelpContactsResponse>.NotFound("Teacher not found");
        }

        var department = await _unitOfWork.Departments.GetByIdAsync(teacher.DepartmentId);


        var response = new TeacherHelpContactsResponse
        {
            DepartmentName = department?.Name,
            DepartmentEmail = department?.Email,
            DepartmentSecretaryEmail = department?.SecretaryEmail,
            DepartmentHeadName = department?.DepartmentHeadName,
            DepartmentHeadEmail = department?.DepartmentHeadEmail,
            RegistrarEmail = _universitySettings.RegistrarEmail,
            AcademicAffairsEmail = _universitySettings.AcademicAffairsEmail,
            HREmail = _universitySettings.HREmail,
            ITSupportEmail = _universitySettings.ITSupportEmail,
            StudentAffairsEmail = _universitySettings.StudentAffairsEmail,
            CounselingEmail = _universitySettings.CounselingEmail,      
        };

        return ResultService<TeacherHelpContactsResponse>.Ok(response);
    }
    public async Task<PagedResultService<AdminUserResponse>> GetAllUsersAsync(string? roleFilter, int? departmentId, string? searchTerm, int pageIndex, int pageSize)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var users = await _unitOfWork.Users.GetAllUsers();

        if (!string.IsNullOrWhiteSpace(roleFilter) && Enum.TryParse<UserRole>(roleFilter, out var role))
        {
            users = users.Where(u => u.Role == role).ToList();
        }
        if (departmentId.HasValue)
        {
            users = users.Where(u => u.DepartmentId == departmentId.Value).ToList();
        }
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            users = users.Where(u =>
                                (u.FirstName != null && u.FirstName.ToLower().Contains(term)) ||
                                (u.LastName != null && u.LastName.ToLower().Contains(term)) ||
                                (u.Email != null && u.Email.ToLower().Contains(term))
                            ).ToList();
        }

        var totalCount = users.Count;

        var pagedUsers = users
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = new List<AdminUserResponse>();
        foreach (var user in pagedUsers)
        {

            var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);
  

            dtos.Add(new AdminUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                DepartmentName = department?.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
        }

        _logger.LogInformation("Retrieved {Count} users. Page {PageIndex}/{TotalPages}",
            dtos.Count, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<AdminUserResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} users"
        );
    }

    public async Task<ResultService<AdminUserResponse>> CreateUserAsync(RegisterRequest request)
    {
        _logger.LogInformation("Attempting to create new user with email: {Email}", request.Email);

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            _logger.LogWarning("User creation failed: email is null or empty");
            return ResultService<AdminUserResponse>.Fail("Email is required");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            _logger.LogWarning("User creation failed: invalid password");
            return ResultService<AdminUserResponse>.Fail("Password must be at least 8 characters");
        }

        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User creation failed: email {Email} already exists", request.Email);
            return ResultService<AdminUserResponse>.Fail($"Email {request.Email} already exists");
        }
        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);

        if (department == null)
        {
            _logger.LogWarning("User creation failed: department {DepartmentId} not found", request.DepartmentId);
            return ResultService<AdminUserResponse>.Fail("Department not found");
        }

        if (!Enum.TryParse<UserRole>(request.Role, out var userRole))
        {
            _logger.LogWarning("User creation failed: invalid role {Role}", request.Role);
            return ResultService<AdminUserResponse>.Fail($"Invalid role: {request.Role}");
        }

        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = userRole,
            DepartmentId = request.DepartmentId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();


        var dept = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);


        var response = new AdminUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            DepartmentName = dept?.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        _logger.LogInformation("User created successfully: {Email} with ID: {UserId}", user.Email, user.Id);
        return ResultService<AdminUserResponse>.Ok(response, "User created successfully");
    }

    public async Task<ResultService<AdminUserResponse>> UpdateUserByAdminAsync(int userId, AdminUpdateUserRequest request)
    {
        _logger.LogInformation("Attempting to update user with ID: {UserId}", userId);

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User update failed: user {UserId} not found", userId);
            return ResultService<AdminUserResponse>.NotFound("User not found");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != userId)
            {
                _logger.LogWarning("User update failed: email {Email} already in use", request.Email);
                return ResultService<AdminUserResponse>.Fail($"Email {request.Email} is already in use");
            }
            user.Email = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrWhiteSpace(request.LastName))
            user.LastName = request.LastName;

        if (Enum.TryParse<UserRole>(request.Role, out var userRole))
            user.Role = userRole;

        if (request.DepartmentId.HasValue)
        {
            if (request.DepartmentId.Value > 0)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId.Value);
                if (department == null)
                {
                    _logger.LogWarning("User update failed: department {DepartmentId} not found", request.DepartmentId);
                    return ResultService<AdminUserResponse>.Fail("Department not found");
                }
            }
            user.DepartmentId = request.DepartmentId.Value;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var dept = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);
        

        var response = new AdminUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            DepartmentName = dept?.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        _logger.LogInformation("User updated successfully: {Email}", user.Email);
        return ResultService<AdminUserResponse>.Ok(response, "User updated successfully");
    }

    public async Task<ResultService> DeleteUserAsync(int userId)
    {
        _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User deletion failed: user {UserId} not found", userId);
            return ResultService.NotFound("User not found");
        }

        var userEmail = user.Email;

        await _unitOfWork.Users.DeleteAsync(userId);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User deleted successfully: {Email}", userEmail);
        return ResultService.Ok($"User {userEmail} deleted successfully");
    }

    public async Task<ResultService<AdminProfileResponse>> GetAdminProfile(int adminId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(adminId);

        if (user == null)
        {
            _logger.LogWarning("Admin with id: {AdminId} does not exist.", adminId);
            return ResultService<AdminProfileResponse>.NotFound("Admin not found");
        }

        var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);

        var profileDto = new AdminProfileResponse
        {
            Id = user.Id,
            Role = (int)user.Role,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,  
            DepartmentId = user.DepartmentId,
            DepartmentName = department?.Name,
        };

        _logger.LogInformation("Retrieved admin profile for user {UserId}", adminId);
        return ResultService<AdminProfileResponse>.Ok(profileDto);
    }
}