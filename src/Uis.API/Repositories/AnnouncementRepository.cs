using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;
public class AnnouncementRepository : BaseRepository<Announcement>, IAnnouncementRepository
{
    public AnnouncementRepository(ApplicationDbContext context) : base(context) {}

    public virtual async Task<List<Announcement>> GetAnnouncementsForStudentAsync(int studentId)
    {
        return await DbSet
            .Include(a => a.TargetCourseInstance)
                .ThenInclude(ci => ci!.Course)
            .Include(a => a.TargetCourseInstance)
                .ThenInclude(ci => ci!.Teacher)
            .Where(a => a.TargetCourseInstanceId.HasValue &&
                        a.TargetCourseInstance!.Enrollments.Any(e =>
                            e.StudentId == studentId &&
                            e.Status == EnrollmentStatus.Active))
            .AsNoTracking()
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }

    public virtual async Task<Announcement?> GetAnnouncementDetailForStudentAsync(int announcementId)
    {
        return await DbSet
                    .Include(a => a.TargetCourseInstance)
                            .ThenInclude(ci => ci!.Course)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == announcementId);
    }

    public virtual async Task<Announcement?> GetAnnouncementDetailForTeacherAsync(int announcementId)
    {
        return await DbSet
            .Include(a => a.TargetCourseInstance)
                .ThenInclude(ci => ci!.Course)
            .FirstOrDefaultAsync(a => a.Id == announcementId);
    }

    public virtual async Task<List<Announcement>> GetTeacherAnnouncementsAsync(int teacherId)
    {
        return await DbSet
            .Where(a => a.CreatedByTeacherId == teacherId)
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }   
  
}
