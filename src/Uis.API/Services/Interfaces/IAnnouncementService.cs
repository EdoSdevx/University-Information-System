using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Announcement;

namespace Uis.API.Services.Interfaces;

public interface IAnnouncementService
{
    Task<PagedResultService<AnnouncementResponse>> GetStudentAnnouncementsAsync(int studentId, int pageIndex = 1, int pageSize = 10);
    Task<ResultService<AnnouncementResponse>> GetAnnouncementDetailForStudentAsync(int announcementId);
    Task<PagedResultService<AnnouncementResponse>> GetTeacherAnnouncementsAsync(int teacherId, int pageIndex = 1, int pageSize = 10);
    Task<ResultService<AnnouncementResponse>> GetAnnouncementDetailForTeacherAsync(int announcementId);
    Task<ResultService<AnnouncementResponse>> CreateAnnouncementAsync(int teacherId, CreateAnnouncementRequest request);
    Task<ResultService<AnnouncementResponse>> UpdateAnnouncementAsync(int announcementId, int teacherId, UpdateAnnouncementRequest request);
    Task<ResultService> DeleteAnnouncementAsync(int announcementId, int teacherId);
}