using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uis.API.DTOs.User;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("by-email/{email}")]
    [Authorize]
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
    [Authorize]
    [ProducesResponseType(typeof(TeacherResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeacherWithCoursesAsync(int teacherId)
    {
        var result = await _userService.GetTeacherWithCoursesAsync(teacherId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}