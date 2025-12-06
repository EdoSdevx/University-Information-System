using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.CourseInstance;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseInstanceController : ControllerBase
{
    private readonly ICourseInstanceService _courseInstanceService;

    public CourseInstanceController(ICourseInstanceService courseInstanceService)
    {
        _courseInstanceService = courseInstanceService;
    }

    // ==================== STUDENT ENDPOINTS ====================

    [HttpGet("my-schedule")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyScheduleAsync()
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _courseInstanceService.GetStudentScheduleAsync(studentId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("available/{academicYearId}")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableCoursesAsync(
        int academicYearId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _courseInstanceService.GetAvailableCoursesAsync(studentId, academicYearId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpGet("{courseInstanceId}/enrollment-detail")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetForEnrollmentAsync(int courseInstanceId)
    {
        var result = await _courseInstanceService.GetForEnrollmentAsync(courseInstanceId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("{courseInstanceId}/has-capacity")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> HasCapacityAsync(int courseInstanceId)
    {
        var result = await _courseInstanceService.HasCapacityAsync(courseInstanceId);
        return Ok(result);
    }

    // ==================== TEACHER ENDPOINTS ====================

    [HttpGet("my-courses")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeacherCoursesAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _courseInstanceService.GetTeacherCoursesAsync(teacherId, pageIndex, pageSize);
        return Ok(result);
    }
}