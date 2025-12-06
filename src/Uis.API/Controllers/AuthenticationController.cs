using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Authentication;
using Uis.API.Services.Interfaces;

namespace Uis.API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Registration request validation failed");
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.RegisterAsync(request);
        if (!result.Success)
        {
            _logger.LogWarning("Registration failed: {Message}", result.Message);
            return StatusCode(result.StatusCode, result);
        }

        _logger.LogInformation("User registered successfully: {Email}", request.Email);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login request validation failed");
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.LoginAsync(request);
        if (!result.Success)
        {
            _logger.LogWarning("Login failed for email: {Email}", request.Email);
            return StatusCode(result.StatusCode, result);
        }

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Refresh token request validation failed");
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.RefreshTokenAsync(request);
        if (!result.Success)
        {
            _logger.LogWarning("Token refresh failed");
            return StatusCode(result.StatusCode, result);
        }

        _logger.LogInformation("Token refreshed successfully");
        return Ok(result);
    }
}
