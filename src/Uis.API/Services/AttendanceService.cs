using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;
public class AttendanceService : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AttendanceService> _logger;

    public AttendanceService(IUnitOfWork unitOfWork, ILogger<AttendanceService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultService> MarkAttendanceAsync(int enrollmentId, MarkAttendanceRequest request)
    {
        try
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                _logger.LogWarning($"Enrollment {enrollmentId} not found");
                return new ResultService { Success = false, Message = "Enrollment not found" };
            }

            var existing = await _unitOfWork.Attendances.GetAttendanceAsync(enrollmentId, request.Week);
            if (existing != null)
            {
                _logger.LogWarning($"Attendance already exists for enrollment {enrollmentId} on {request.Week}");
                return new ResultService { Success = false, Message = "Attendance already recorded for this date" };
            }

            var attendance = new Attendance
            {
                EnrollmentId = enrollmentId,
                Week = request.Week,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Attendances.AddAsync(attendance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Attendance marked for enrollment {enrollmentId}");
            return new ResultService { Success = true, Message = "Attendance marked successfully" };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error marking attendance: {ex.Message}");
            return new ResultService { Success = false, Message = "An error occurred while marking attendance" };
        }
    }

    public async Task<PagedResultService<AttendanceResponse>> GetStudentAttendanceAsync(int studentId, int pageIndex, int pageSize)
    {
        try
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var attendances = await _unitOfWork.Attendances.GetStudentAttendanceAsync(studentId);

            var total = attendances.Count;
            var paginated = attendances
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = paginated.Select(a => new AttendanceResponse
            {
                Id = a.Id,
                CourseCode = a.Enrollment?.CourseInstance?.Course?.Code ?? "N/A",
                CourseName = a.Enrollment?.CourseInstance?.Course?.Name ?? "N/A",
                Week = a.Week,
                Status = a.Status.ToString(),
                CreatedAt = a.CreatedAt
            }).ToList();

            return new PagedResultService<AttendanceResponse>
            {
                Success = true,
                Data = dtos,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting student attendance: {ex.Message}");
            return new PagedResultService<AttendanceResponse>
            {
                Success = false,
                Message = "An error occurred while retrieving attendance records"
            };
        }
    }

    public async Task<PagedResultService<StudentAttendanceResponse>> GetCourseAttendanceAsync(int courseInstanceId, int week , int pageIndex, int pageSize)
    {
        try
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var attendances = await _unitOfWork.Attendances.GetCourseAttendanceAsync(courseInstanceId, week);

            var total = attendances.Count;
            var paginated = attendances
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = paginated.Select(a => new StudentAttendanceResponse
            {
                Id = a.Id,
                EnrollmentId = a.EnrollmentId,
                StudentName = $"{a.Enrollment?.Student?.FirstName} {a.Enrollment?.Student?.LastName}",
                Week = a.Week,
                Day = a.Day,
                Status = a.Status
            }).ToList();

            return new PagedResultService<StudentAttendanceResponse>
            {
                Success = true,
                Data = dtos,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course attendance: {ex.Message}");
            return new PagedResultService<StudentAttendanceResponse>
            {
                Success = false,
                Message = "An error occurred while retrieving course attendance"
            };
        }
    }

    public async Task<ResultService> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceRequest request)
    {
        try
        {
            var attendance = await _unitOfWork.Attendances.GetByIdAsync(attendanceId);

            if (attendance == null)
            {
                _logger.LogWarning($"Attendance {attendanceId} not found");
                return new ResultService { Success = false, Message = "Attendance record not found" };
            }

            attendance.Status = request.Status;
            attendance.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Attendances.UpdateAsync(attendance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Attendance {attendanceId} updated successfully");
            return new ResultService { Success = true, Message = "Attendance updated successfully" };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating attendance: {ex.Message}");
            return new ResultService { Success = false, Message = "An error occurred while updating attendance" };
        }
    }

    public async Task<ResultService> DeleteAttendanceAsync(int attendanceId)
    {
        try
        {
            var attendance = await _unitOfWork.Attendances.GetByIdAsync(attendanceId);
            if (attendance == null)
            {
                _logger.LogWarning($"Attendance {attendanceId} not found");
                return new ResultService { Success = false, Message = "Attendance record not found" };
            }

            await _unitOfWork.Attendances.DeleteAsync(attendance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Attendance {attendanceId} deleted successfully");
            return new ResultService { Success = true, Message = "Attendance deleted successfully" };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting attendance: {ex.Message}");
            return new ResultService { Success = false, Message = "An error occurred while deleting attendance" };
        }
    }
}
