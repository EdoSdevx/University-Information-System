using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Department;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IUnitOfWork unitOfWork, ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultService<DepartmentResponse>> GetDepartmentByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve department with ID: {DepartmentId}", id);

        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            _logger.LogWarning("Department with ID {DepartmentId} not found", id);
            return ResultService<DepartmentResponse>.NotFound($"Department with ID {id} does not exist");
        }

        var deptDto = MapToDepartmentResponse(department);
        _logger.LogInformation("Successfully retrieved department: {DepartmentCode}", department.Code);
        return ResultService<DepartmentResponse>.Ok(deptDto, "Department retrieved successfully");
    }

    public async Task<ResultService<DepartmentResponse>> GetDepartmentByCodeAsync(string code)
    {
        _logger.LogInformation("Attempting to retrieve department with code: {Code}", code);

        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogWarning("Department code validation failed: code is null or empty");
            return ResultService<DepartmentResponse>.Fail("Department code cannot be empty", ResultErrorCode.ValidationError);
        }

        var department = await _unitOfWork.Departments.GetByCodeAsync(code);
        if (department == null)
        {
            _logger.LogWarning("Department with code {Code} not found", code);
            return ResultService<DepartmentResponse>.NotFound($"Department with code {code} does not exist");
        }

        var deptDto = MapToDepartmentResponse(department);
        _logger.LogInformation("Successfully retrieved department: {DepartmentCode}", code);
        return ResultService<DepartmentResponse>.Ok(deptDto, "Department retrieved successfully");
    }
    public async Task<ResultService<DepartmentDetailResponse>> GetDepartmentWithCoursesAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve department with courses. Department ID: {DepartmentId}", id);

        var department = await _unitOfWork.Departments.GetWithCoursesAsync(id);
        if (department == null)
        {
            _logger.LogWarning("Department with ID {DepartmentId} not found", id);
            return ResultService<DepartmentDetailResponse>.NotFound($"Department with ID {id} does not exist");
        }

        var deptDetailDto = MapToDepartmentDetailResponse(department);
        var courseCount = department.Courses?.Count ?? 0;
        _logger.LogInformation("Successfully retrieved department {Code} with {CourseCount} courses", department.Code, courseCount);
        return ResultService<DepartmentDetailResponse>.Ok(deptDetailDto, "Department with courses retrieved successfully");
    }

    public async Task<ResultService<DepartmentResponse>> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        _logger.LogInformation("Attempting to create new department with code: {Code}", request?.Code);

        if (request == null)
        {
            _logger.LogWarning("Department creation failed: request is null");
            return ResultService<DepartmentResponse>.Fail("Department data is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            _logger.LogWarning("Department creation failed: code is null or empty");
            return ResultService<DepartmentResponse>.Fail("Department code is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogWarning("Department creation failed: name is null or empty");
            return ResultService<DepartmentResponse>.Fail("Department name is required", ResultErrorCode.ValidationError);
        }

        var existingDept = await _unitOfWork.Departments.GetByCodeAsync(request.Code);
        if (existingDept != null)
        {
            _logger.LogWarning("Department creation failed: code {Code} already exists", request.Code);
            return ResultService<DepartmentResponse>.Fail($"Department code {request.Code} already exists", ResultErrorCode.Conflict);
        }

        var department = new Department
        {
            Code = request.Code,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var deptDto = MapToDepartmentResponse(department);
        _logger.LogInformation("Department created successfully: {Code} with ID: {DepartmentId}", department.Code, department.Id);
        return ResultService<DepartmentResponse>.Ok(deptDto, $"Department {department.Code} created successfully");
    }

    public async Task<ResultService<DepartmentResponse>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request)
    {
        _logger.LogInformation("Attempting to update department with ID: {DepartmentId}", id);

        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            _logger.LogWarning("Department update failed: department with ID {DepartmentId} not found", id);
            return ResultService<DepartmentResponse>.NotFound($"Department with ID {id} does not exist");
        }

        if (request == null)
        {
            _logger.LogWarning("Department update failed: request is null");
            return ResultService<DepartmentResponse>.Fail("Updated department data is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogWarning("Department update failed: name is null or empty for department {DepartmentId}", id);
            return ResultService<DepartmentResponse>.Fail("Department name is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            _logger.LogWarning("Department update failed: code is null or empty for department {DepartmentId}", id);
            return ResultService<DepartmentResponse>.Fail("Department code is required", ResultErrorCode.ValidationError);
        }

        var codeExists = await _unitOfWork.Departments.GetByCodeAsync(request.Code);
        if (codeExists != null && codeExists.Id != id)
        {
            _logger.LogWarning("Department update failed: code {Code} already in use", request.Code);
            return ResultService<DepartmentResponse>.Fail($"Department code {request.Code} is already in use", ResultErrorCode.Conflict);
        }

        department.Name = request.Name;
        department.Code = request.Code;
        department.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Departments.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var deptDto = MapToDepartmentResponse(department);
        _logger.LogInformation("Department updated successfully: {Code}", department.Code);
        return ResultService<DepartmentResponse>.Ok(deptDto, $"Department {department.Code} updated successfully");
    }

    public async Task<ResultService> DeleteDepartmentAsync(int id)
    {
        _logger.LogInformation("Attempting to delete department with ID: {DepartmentId}", id);

        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            _logger.LogWarning("Department deletion failed: department with ID {DepartmentId} not found", id);
            return ResultService.NotFound($"Department with ID {id} does not exist");
        }

        var departmentCode = department.Code;
        await _unitOfWork.Departments.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Department deleted successfully: {Code}", departmentCode);
        return ResultService.Ok($"Department {departmentCode} deleted successfully");
    }

    // ==================== MAPPING METHODS ====================
    private static DepartmentResponse MapToDepartmentResponse(Department dept)
    {
        return new DepartmentResponse
        {
            Id = dept.Id,
            Name = dept.Name,
            Code = dept.Code,
            CreatedAt = dept.CreatedAt,
            UpdatedAt = dept.UpdatedAt
        };
    }

    private static DepartmentDetailResponse MapToDepartmentDetailResponse(Department dept)
    {
        return new DepartmentDetailResponse
        {
            Id = dept.Id,
            Name = dept.Name,
            Code = dept.Code,
            CourseCount = dept.Courses?.Count ?? 0,
            CreatedAt = dept.CreatedAt,
            UpdatedAt = dept.UpdatedAt
        };
    }
}