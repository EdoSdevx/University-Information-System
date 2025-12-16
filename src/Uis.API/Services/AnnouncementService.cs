using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Announcement;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AnnouncementService> _logger;

    public AnnouncementService(IUnitOfWork unitOfWork, ILogger<AnnouncementService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResultService<AnnouncementResponse>> GetStudentAnnouncementsAsync(int studentId, int pageIndex = 1, int pageSize = 10)
    {
        if (pageIndex < 1) pageIndex = 1;

        if (pageSize < 1) pageSize = 10;

        var announcements = await _unitOfWork.Announcements.GetAnnouncementsForStudentAsync(studentId);

        var totalCount = announcements.Count;

        var pagedAnnouncements = announcements
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

        var dtos = pagedAnnouncements.Select(a => new AnnouncementResponse
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            TargetCourseInstanceId = a.TargetCourseInstanceId ?? 0,
            CreatedByTeacherName = a.CreatedByTeacherName,
            CourseCode = a.TargetCourseInstance?.Course?.Code ?? null,
            CourseName = a.TargetCourseInstance?.Course?.Name ?? null,
            PublishedAt = a.PublishedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} announcements for student {StudentId}", totalCount, studentId);
        return PagedResultService<AnnouncementResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} announcements"
        );
    }
    public async Task<ResultService<AnnouncementResponse>> GetAnnouncementDetailForStudentAsync(int announcementId)
    {

        var announcement = await _unitOfWork.Announcements.GetAnnouncementDetailForStudentAsync(announcementId);

        if (announcement == null)
        {
            _logger.LogWarning("Announcement {AnnouncementId} not found", announcementId);
            return ResultService<AnnouncementResponse>.NotFound("Announcement not found");
        }

        var dto = new AnnouncementResponse
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            TargetCourseInstanceId = announcement.TargetCourseInstanceId ?? 0,
            CreatedByTeacherName = announcement.CreatedByTeacherName,
            CourseCode = announcement.TargetCourseInstance?.Course?.Code ?? null,
            CourseName = announcement.TargetCourseInstance?.Course?.Name ?? null,
            PublishedAt = announcement.PublishedAt
        };

        return ResultService<AnnouncementResponse>.Ok(dto);
    }
    public async Task<ResultService<AnnouncementResponse>> GetAnnouncementDetailForTeacherAsync(int announcementId, int teacherId)
    {
        var announcement = await _unitOfWork.Announcements.GetAnnouncementDetailForTeacherAsync(announcementId);

        if (announcement == null)
        {
            _logger.LogWarning("Announcement {AnnouncementId} not found", announcementId);
            return ResultService<AnnouncementResponse>.NotFound("Announcement not found");
        }

        if(teacherId != announcement.CreatedByTeacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} tried to get detail of announcement: {AnnouncementId}.", teacherId,announcementId);
            return ResultService<AnnouncementResponse>.Fail("You can only view your own announcements.");
        }
        var dto = new AnnouncementResponse
        {
            Id = announcement.Id,
            CreatedByTeacherId = announcement.CreatedByTeacherId,
            TargetCourseInstanceId = announcement.TargetCourseInstanceId ?? 0,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedByTeacherName = announcement.CreatedByTeacherName,
            CourseCode = announcement.TargetCourseInstance?.Course?.Code ?? null,
            CourseName = announcement.TargetCourseInstance?.Course?.Name ?? null,
            PublishedAt = announcement.PublishedAt
        };

        return ResultService<AnnouncementResponse>.Ok(dto);
    }

    public async Task<PagedResultService<AnnouncementResponse>> GetTeacherAnnouncementsAsync(int teacherId, int pageIndex = 1, int pageSize = 10)
    {
        if (pageIndex < 1) pageIndex = 1;

        if (pageSize < 1) pageSize = 10;

        var announcements = await _unitOfWork.Announcements.GetTeacherAnnouncementsAsync(teacherId);

        var totalCount = announcements.Count;

        var pagedAnnouncements = announcements
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

        var dtos = pagedAnnouncements.Select(a => new AnnouncementResponse
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            TargetCourseInstanceId = a.TargetCourseInstanceId ?? 0,
            CreatedByTeacherName = a.CreatedByTeacherName,
            CourseCode = a.TargetCourseInstance?.Course?.Code ?? "N/A",
            CourseName = a.TargetCourseInstance?.Course?.Name ?? "N/A",
            PublishedAt = a.PublishedAt
        }).ToList();

        _logger.LogInformation("Retrieved {Count} announcements for teacher {TeacherId}. Page {PageIndex}/{TotalPages}",
            dtos.Count, teacherId, pageIndex, (int)Math.Ceiling((double)totalCount / pageSize));

        return PagedResultService<AnnouncementResponse>.Ok(
            dtos,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount,
            $"Retrieved {totalCount} announcements"
        );
    }

    public async Task<ResultService<AnnouncementResponse>> CreateAnnouncementAsync(int teacherId, CreateAnnouncementRequest request)
    {

        if (request == null)
            return ResultService<AnnouncementResponse>.Fail("Announcement data required");

        var teacher = await _unitOfWork.Users.GetByIdAsync(teacherId);
        if (teacher == null)
        {
            _logger.LogWarning("Teacher {TeacherId} not found", teacherId);
            return ResultService<AnnouncementResponse>.Fail("Teacher not found");
        }

        var announcement = new Announcement
        {
            Title = request.Title,
            Content = request.Content,
            CreatedByTeacherId = teacherId,
            CreatedByTeacherName = $"{teacher.FirstName} {teacher.LastName}",
            TargetCourseInstanceId = request.TargetCourseInstanceId,
            PublishedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Announcements.AddAsync(announcement);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Announcement created: {Title} by teacher {TeacherId}", announcement.Title, teacherId);

        var dto = new AnnouncementResponse
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedByTeacherName = announcement.CreatedByTeacherName,
            CourseCode = announcement.TargetCourseInstance?.Course?.Code ?? null,
            CourseName = announcement?.TargetCourseInstance?.Course?.Name ?? null,
            PublishedAt = announcement.PublishedAt
        };
        return ResultService<AnnouncementResponse>.Ok(dto, "Announcement created");
    }
    public async Task<ResultService<AnnouncementResponse>> UpdateAnnouncementAsync(int announcementId, int teacherId, UpdateAnnouncementRequest request)
    {
        var announcement = await _unitOfWork.Announcements.GetByIdAsync(announcementId);

        if (announcement == null)
        {
            _logger.LogWarning("Announcement {AnnouncementId} not found", announcementId);
            return ResultService<AnnouncementResponse>.NotFound("Announcement not found");
        }

        if (announcement.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} attempted to edit announcement by {CreatedByTeacherId}",
                teacherId, announcement.CreatedByTeacherId);
            return ResultService<AnnouncementResponse>.Fail("You can only edit your own announcements", ResultErrorCode.Forbidden);
        }

        announcement.Title = request.Title ?? announcement.Title;
        announcement.Content = request.Content ?? announcement.Content;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Announcements.UpdateAsync(announcement);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Announcement {AnnouncementId} updated", announcementId);
        var dto = new AnnouncementResponse
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedByTeacherName = announcement.CreatedByTeacherName,
            CourseCode = announcement.TargetCourseInstance?.Course?.Code ?? null,
            CourseName = announcement?.TargetCourseInstance?.Course?.Name ?? null,
            PublishedAt = announcement.PublishedAt
        };
        return ResultService<AnnouncementResponse>.Ok(dto, "Announcement updated");
    }
    public async Task<ResultService> DeleteAnnouncementAsync(int announcementId, int teacherId)
    {
        var announcement = await _unitOfWork.Announcements.GetByIdAsync(announcementId);

        if (announcement == null)
        {
            _logger.LogWarning("Announcement {AnnouncementId} not found", announcementId);
            return ResultService.NotFound("Announcement not found");
        }

        if (announcement.CreatedByTeacherId != teacherId)
        {
            _logger.LogWarning("Teacher {TeacherId} attempted to delete announcement by {CreatedByTeacherId}",
                teacherId, announcement.CreatedByTeacherId);
            return ResultService.Fail("You can only delete your own announcements", ResultErrorCode.Forbidden);
        }

        await _unitOfWork.Announcements.DeleteAsync(announcement);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Announcement {AnnouncementId} deleted", announcementId);
        return ResultService.Ok("Announcement deleted");
    }

}