using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Grade;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    // ==================== STUDENT ENDPOINTS ====================

    [HttpGet("my-grades")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentGradesAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _gradeService.GetStudentGradesAsync(studentId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpGet("my-grade/{courseInstanceId}")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentCourseGradeAsync(int courseInstanceId)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _gradeService.GetStudentCourseGradeAsync(studentId, courseInstanceId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    // ==================== TEACHER ENDPOINTS ====================

    [HttpGet("course/{courseInstanceId}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseGradesAsync(
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

        var result = await _gradeService.GetCourseGradesAsync(courseInstanceId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignGradeAsync([FromBody] AssignGradeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _gradeService.AssignGradeAsync(request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{gradeId}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGradeAsync(int gradeId, [FromBody] UpdateGradeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _gradeService.UpdateGradeAsync(gradeId, teacherId, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpDelete("{gradeId}")]
    [Authorize(Roles = Roles.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGradeAsync(int gradeId)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _gradeService.DeleteGradeAsync(gradeId, teacherId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}