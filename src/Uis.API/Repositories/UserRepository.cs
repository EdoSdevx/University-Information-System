using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public virtual async Task<User?> GetTeacherWithCoursesAsync(int teacherId)
        {
            return await DbSet
                .Include(u => u.TaughtCourses)
                .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher);
        }

        public virtual async Task<List<User>> GetAllUsers()
        {
            return await DbSet
                        .Include(u => u.Department)
                        .OrderByDescending(u => u.CreatedAt)
                        .ToListAsync();
        }
    }
}