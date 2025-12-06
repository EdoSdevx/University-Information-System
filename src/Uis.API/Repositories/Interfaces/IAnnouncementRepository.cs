using System;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces
{
    public interface IAnnouncementRepository : IBaseRepository<Announcement>
    {
        Task<List<Announcement>> GetAnnouncementsForStudentAsync(int studentId);
        Task<Announcement> GetAnnouncementDetailForStudentAsync(int announcementId);
        Task<List<Announcement>> GetTeacherAnnouncementsAsync(int teacherId);
        Task<Announcement> GetAnnouncementDetailForTeacherAsync(int announcementId);
    }
}