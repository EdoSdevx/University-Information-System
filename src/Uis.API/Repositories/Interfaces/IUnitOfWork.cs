using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Repositories.Interfaces;
public interface IUnitOfWork : IAsyncDisposable
{
    IUserRepository Users { get; }
    IDepartmentRepository Departments { get; }
    ICourseRepository Courses { get; }
    ICourseInstanceRepository CourseInstances { get; }
    IAcademicYearRepository AcademicYears { get; }
    IEnrollmentRepository Enrollments { get; }
    IGradeRepository Grades { get; }
    IAnnouncementRepository Announcements { get; }
    IAttendanceRepository Attendances { get; }

    Task<int> SaveChangesAsync();
    Task<IAsyncDisposable> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

}
