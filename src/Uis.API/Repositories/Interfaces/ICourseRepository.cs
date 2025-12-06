using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<Course?> GetByCodeAsync(string code);
        Task<List<Course>> GetByDepartmentAsync(int departmentId);
    }
}