using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Uis.API.DTOs;
using Uis.API.DTOs.Authentication;
using Uis.API.DTOs.User;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService, IUnitOfWork unitOfWork)
    {
        _userService = userService;
    }

    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByEmailAsync(string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("teacher/{teacherId}/courses")]
    [ProducesResponseType(typeof(TeacherResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeacherWithCoursesAsync(int teacherId)
    {
        var result = await _userService.GetTeacherWithCoursesAsync(teacherId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(StudentProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.GetStudentProfileAsync(userId);   
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] StudentUpdateProfileRequest model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.UpdateStudentProfileAsync(userId, model);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
       
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.ChangePasswordAsync(userId, model);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(new { message = result.Message });
    }

    [HttpGet("help-contacts")]
    public async Task<IActionResult> GetHelpContacts()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.GetHelpContactsAsync(userId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
    [HttpGet("profile/teacher")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<TeacherProfileResponse>>> GetTeacherProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.GetTeacherProfileAsync(userId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPut("profile/teacher")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<TeacherProfileResponse>>> UpdateTeacherProfile([FromBody] TeacherUpdateProfileRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.UpdateTeacherProfileAsync(userId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
    [HttpGet("help-contacts/teacher")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<TeacherHelpContactsResponse>>> GetTeacherHelpContacts()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized("You are not authorized.");
        }

        var userId = int.Parse(userIdClaim);

        var result = await _userService.GetTeacherHelpContactsAsync(userId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers([FromQuery] string? roleFilter,
        [FromQuery] int? departmentId,
        [FromQuery] string? searchTerm, [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetAllUsersAsync(roleFilter, departmentId, searchTerm, pageIndex, pageSize);
        return Ok(result);
    }

    [HttpPost("admin/create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
    {
        var result = await _userService.CreateUserAsync(request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("admin/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserByAdmin(int userId, [FromBody] AdminUpdateUserRequest request)
    {
        var result = await _userService.UpdateUserByAdminAsync(userId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpDelete("admin/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}