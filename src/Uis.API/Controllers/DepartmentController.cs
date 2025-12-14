using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Department;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllDepartments(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _departmentService.GetAllDepartmentsAsync(pageIndex, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartmentByIdAsync(int id)
    {
        var result = await _departmentService.GetDepartmentByIdAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("by-code/{code}")]
    [Authorize]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartmentByCodeAsync(string code)
    {
        var result = await _departmentService.GetDepartmentByCodeAsync(code);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpGet("{id}/with-courses")]
    [Authorize]
    [ProducesResponseType(typeof(DepartmentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartmentWithCoursesAsync(int id)
    {
        var result = await _departmentService.GetDepartmentWithCoursesAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _departmentService.CreateDepartmentAsync(request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDepartmentAsync(int id, [FromBody] UpdateDepartmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _departmentService.UpdateDepartmentAsync(id, request);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDepartmentAsync(int id)
    {
        var result = await _departmentService.DeleteDepartmentAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}