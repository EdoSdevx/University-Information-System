using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Department;

namespace Uis.API.Services.Interfaces;

public interface IDepartmentService
{
    Task<ResultService<DepartmentResponse>> GetDepartmentByIdAsync(int id);
    Task<ResultService<DepartmentResponse>> GetDepartmentByCodeAsync(string code);
    Task<ResultService<DepartmentDetailResponse>> GetDepartmentWithCoursesAsync(int id);
    Task<ResultService<DepartmentResponse>> CreateDepartmentAsync(CreateDepartmentRequest request);
    Task<ResultService<DepartmentResponse>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request);
    Task<ResultService> DeleteDepartmentAsync(int id);
    Task<PagedResultService<DepartmentResponse>> GetAllDepartmentsAsync(int pageIndex, int pageSize);
}