using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department?> GetByCodeAsync(string code);
        Task<Department?> GetWithCoursesAsync(int departmentId);
    }
}