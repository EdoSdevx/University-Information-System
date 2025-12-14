using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs;
using Uis.API.DTOs.Authentication;
using Uis.API.DTOs.User;
using Uis.API.Models;

namespace Uis.API.Services.Interfaces;

public interface IUserService
{
    Task<ResultService<UserResponse>> GetUserByIdAsync(int id);
    Task<ResultService<UserResponse>> GetUserByEmailAsync(string email);
    Task<ResultService<TeacherResponse>> GetTeacherWithCoursesAsync(int teacherId);
    Task<ResultService<StudentProfileResponse>> GetStudentProfileAsync(int studentId);
    Task<ResultService<StudentProfileResponse>> UpdateStudentProfileAsync(int studentId, StudentUpdateProfileRequest request);
    Task<ResultService<bool>> ChangePasswordAsync(int userId, ChangePasswordRequest model);
    Task<ResultService<HelpContactsResponse>> GetHelpContactsAsync(int studentId);
    Task<ResultService<TeacherProfileResponse>> GetTeacherProfileAsync(int teacherId);
    Task<ResultService<TeacherProfileResponse>> UpdateTeacherProfileAsync(int teacherId, TeacherUpdateProfileRequest request);
    Task<ResultService<TeacherHelpContactsResponse>> GetTeacherHelpContactsAsync(int teacherId);

    Task<PagedResultService<AdminUserResponse>> GetAllUsersAsync(string? roleFilter, int? departmentId, string? searchTerm, int pageIndex, int pageSize);
    Task<ResultService<AdminUserResponse>> CreateUserAsync(RegisterRequest request);
    Task<ResultService<AdminUserResponse>> UpdateUserByAdminAsync(int userId, AdminUpdateUserRequest request);
    Task<ResultService> DeleteUserAsync(int userId);
}
