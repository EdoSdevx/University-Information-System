using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Enrollment;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    // ==================== STUDENT ENDPOINTS ====================

    [HttpGet("my-enrollments")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentEnrollmentsAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _enrollmentService.GetStudentEnrollmentsAsync(studentId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EnrollStudentAsync([FromBody] EnrollStudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _enrollmentService.EnrollStudentAsync(studentId, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("drop")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DropCourseAsync([FromBody] DropCourseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _enrollmentService.DropCourseAsync(studentId, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    // ==================== TEACHER ENDPOINTS ====================

    [HttpGet("course/{courseInstanceId}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseEnrollmentsAsync(
        int courseInstanceId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _enrollmentService.GetCourseEnrollmentsAsync(courseInstanceId, pageIndex, pageSize);
        return Ok(result);
    }
}