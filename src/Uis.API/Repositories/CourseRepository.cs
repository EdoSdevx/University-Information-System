using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<Course?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public virtual async Task<List<Course>> GetByDepartmentAsync(int departmentId)
        {
            return await DbSet
                .Where(c => c.DepartmentId == departmentId)
                .AsNoTracking()
                .OrderBy(c => c.Code)
                .ToListAsync();
        }
    }
}