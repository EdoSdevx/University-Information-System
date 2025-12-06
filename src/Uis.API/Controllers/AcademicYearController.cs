using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.AcademicYear;
using Uis.API.Services.Interfaces;

namespace Uis.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcademicYearController : ControllerBase
{
    private readonly IAcademicYearService _academicYearService;

    public AcademicYearController(IAcademicYearService academicYearService)
    {
        _academicYearService = academicYearService;
    }

    [HttpGet("active")]
    [Authorize]
    [ProducesResponseType(typeof(AcademicYearResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveAcademicYearAsync()
    {
        var result = await _academicYearService.GetActiveAcademicYearAsync();
        if (!result.Success)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }

}