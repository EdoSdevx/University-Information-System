using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uis.API.Data;
using Uis.API.Models;

namespace Uis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoadTestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LoadTestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("seed")]
    [AllowAnonymous]
    public async Task<IActionResult> Seed([FromQuery] int students = 5000, [FromQuery] int capacity = 50)
    {
#if !DEBUG
        return NotFound();
#endif
        await LoadTestSeed.SeedAsync(_context, students, capacity);
        return Ok(new { message = "Load test data seeded", students, capacity });
    }

    [HttpPost("reset")]
    [AllowAnonymous]
    public async Task<IActionResult> Reset()
    {
#if !DEBUG
        return NotFound();
#endif
        await LoadTestSeed.ResetAsync(_context);
        return Ok(new { message = "Load test enrollments reset" });
    }
}