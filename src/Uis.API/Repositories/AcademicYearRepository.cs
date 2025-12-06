using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class AcademicYearRepository : BaseRepository<AcademicYear>, IAcademicYearRepository
    {
        public AcademicYearRepository(ApplicationDbContext context) : base(context) { }


        public virtual async Task<AcademicYear?> GetActiveAsync()
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(ay => ay.IsActive);
        }

    }
}