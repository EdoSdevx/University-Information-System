using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<Department?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public virtual async Task<Department?> GetWithCoursesAsync(int departmentId)
        {
            return await DbSet
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.Id == departmentId);
        }
    }
}