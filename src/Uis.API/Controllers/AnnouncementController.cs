using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Announcement;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    private readonly IEnrollmentService _enrollmentService;

    public AnnouncementController(IAnnouncementService announcementService, IEnrollmentService enrollmentService)
    {
        _announcementService = announcementService;
        _enrollmentService = enrollmentService;
    }

    // ==================== STUDENT ENDPOINTS ====================

    [HttpGet("my-announcements")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAnnouncementsAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _announcementService.GetStudentAnnouncementsAsync(studentId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}/detail")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAnnouncementDetailAsync(int id)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var announcementResult = await _announcementService.GetAnnouncementDetailForStudentAsync(id);
        if (!announcementResult.Success)
            return StatusCode(announcementResult.StatusCode, announcementResult);

        var announcement = announcementResult.Data;

        var isEnrolled = await _enrollmentService.IsStudentEnrolledAsync(studentId, announcement.TargetCourseInstanceId);

        if (!isEnrolled)
        {
            return Forbid("You are not enrolled in this course.");
        }

        return Ok(announcementResult);
    }

    // ==================== TEACHER ENDPOINTS ====================

    [HttpGet("my-announcements-teacher")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeacherAnnouncementsAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _announcementService.GetTeacherAnnouncementsAsync(teacherId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAnnouncementAsync([FromBody] CreateAnnouncementRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _announcementService.CreateAnnouncementAsync(teacherId, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAnnouncementAsync(
        int id,
        [FromBody] UpdateAnnouncementRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var getResult = await _announcementService.GetAnnouncementDetailForTeacherAsync(id);
        if (!getResult.Success)
            return StatusCode(getResult.StatusCode, getResult);

        if (getResult.Data.CreatedByTeacherId != teacherId)
        {
            return Forbid("You can only edit your own announcements.");
        }

        var result = await _announcementService.UpdateAnnouncementAsync(id, teacherId, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAnnouncementAsync(int id)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var getResult = await _announcementService.GetAnnouncementDetailForTeacherAsync(id);
        if (!getResult.Success)
            return StatusCode(getResult.StatusCode, getResult);

        if (getResult.Data.CreatedByTeacherId != teacherId)
        {
            return Forbid("You can only delete your own announcements.");
        }

        var result = await _announcementService.DeleteAnnouncementAsync(id, teacherId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}