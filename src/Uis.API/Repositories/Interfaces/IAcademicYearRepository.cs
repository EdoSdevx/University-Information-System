using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface IAcademicYearRepository : IBaseRepository<AcademicYear>
    {
        Task<AcademicYear?> GetActiveAsync();
    }
}