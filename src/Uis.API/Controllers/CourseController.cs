using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.DTOs.Course;
using Uis.API.Services;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IUserService _userService;

    public CourseController(ICourseService courseService, IUserService userService)
    {
        _courseService = courseService;
        _userService = userService;
    }

    [HttpGet("admin/by-department/{departmentId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDepartmentForAdminAsync(
    int departmentId,
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 100)
    {
        var result = await _courseService.GetByDepartmentAsync(departmentId, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCoursesForAdminAsync(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 100)
    {
        var result = await _courseService.GetAllCoursesAsync(pageIndex, pageSize);
        return Ok(result);
    }

    [HttpPost("admin/create")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CourseDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCourseAsync([FromBody] CreateCourseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _courseService.CreateCourseAsync(request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPut("admin/edit/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseAsync(int id, [FromBody] UpdateCourseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _courseService.UpdateCourseAsync(id, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete("admin/delete/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseAsync(int id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
    [HttpGet("by-code/{code}")]
    [Authorize]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseByCodeAsync(string code)
    {
        var result = await _courseService.GetCourseByCodeAsync(code);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("by-department/{departmentId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDepartmentAsync(
        int departmentId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var userResult = await _userService.GetUserByIdAsync(userId);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult);

        var user = userResult.Data;

        if (user!.DepartmentId != departmentId)
        {
            return StatusCode(StatusCodes.Status403Forbidden,
            new { success = false, message = "You can only view courses from your own department." });
        }

        var result = await _courseService.GetByDepartmentAsync(departmentId, pageIndex, pageSize);
        return Ok(result);
    }
}