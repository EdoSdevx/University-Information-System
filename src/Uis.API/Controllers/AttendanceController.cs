using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs;
using Uis.API.Models;
using Uis.API.Services.Interfaces;

namespace Uis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(IAttendanceService attendanceService, ILogger<AttendanceController> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    [HttpPost("mark")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _attendanceService.MarkAttendanceAsync(request.EnrollmentId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("my-attendances")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetStudentAttendance([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(studentIdClaim, out int studentId))
            return Unauthorized();

        var result = await _attendanceService.GetStudentAttendanceAsync(studentId, pageIndex, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("course/{courseInstanceId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetCourseAttendance(
        int courseInstanceId,
        [FromQuery] DateTime? date = null,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        date ??= DateTime.UtcNow.Date;
        var result = await _attendanceService.GetCourseAttendanceAsync(courseInstanceId, date.Value, pageIndex, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{attendanceId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] UpdateAttendanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _attendanceService.UpdateAttendanceAsync(attendanceId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{attendanceId}")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> DeleteAttendance(int attendanceId)
    {
        var result = await _attendanceService.DeleteAttendanceAsync(attendanceId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}