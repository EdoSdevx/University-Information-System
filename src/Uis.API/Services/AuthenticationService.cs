using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.Authentication;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;
public class AuthenticationService : Interfaces.IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<AuthenticationService> logger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _logger = logger;
    }
    public async Task<ResultService> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Admin attempting to register new user with email: {Email}", request?.Email);

        if (request == null)
        {
            _logger.LogWarning("Registration failed: request is null");
            return ResultService<AuthenticationResponse>.Fail("Registration data is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            _logger.LogWarning("Registration failed: email is null or empty");
            return ResultService<AuthenticationResponse>.Fail("Email is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            _logger.LogWarning("Registration failed: password is weak");
            return ResultService<AuthenticationResponse>.Fail("Password must be at least 6 characters", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            _logger.LogWarning("Registration failed: first name is null or empty");
            return ResultService<AuthenticationResponse>.Fail("First name is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            _logger.LogWarning("Registration failed: last name is null or empty");
            return ResultService<AuthenticationResponse>.Fail("Last name is required", ResultErrorCode.ValidationError);
        }

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            return ResultService<AuthenticationResponse>.Fail(
                "Invalid role. Must be: Admin, Student, or Teacher",
                ResultErrorCode.ValidationError);
        }
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: email {Email} already exists", request.Email);
            return ResultService<AuthenticationResponse>.Fail("Email already registered", ResultErrorCode.Conflict);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User registered successfully by admin: {Email} as {Role}", user.Email, user.Role);
        return ResultService<AuthenticationResponse>.Ok("User registered successfully");
    }

    public async Task<ResultService<AuthenticationResponse>> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Attempting to login user with email: {Email}", request?.Email);

        if (request == null)
        {
            _logger.LogWarning("Login failed: request is null");
            return ResultService<AuthenticationResponse>.Fail("Login credentials are required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            _logger.LogWarning("Login failed: email is null or empty");
            return ResultService<AuthenticationResponse>.Fail("Email is required", ResultErrorCode.ValidationError);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login failed: password is null or empty");
            return ResultService<AuthenticationResponse>.Fail("Password is required", ResultErrorCode.ValidationError);
        }

        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: user with email {Email} not found", request.Email);
            return ResultService<AuthenticationResponse>.Fail("Invalid email or password", ResultErrorCode.ValidationError);
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed: incorrect password for user {Email}", request.Email);
            return ResultService<AuthenticationResponse>.Fail("Invalid email or password", ResultErrorCode.ValidationError);
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var response = new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        _logger.LogInformation("User logged in successfully: {Email}", user.Email);
        return ResultService<AuthenticationResponse>.Ok(response, "Login successful");
    }

    public async Task<ResultService<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        _logger.LogInformation("Attempting to refresh token");

        if (request == null || string.IsNullOrWhiteSpace(request.AccessToken) || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            _logger.LogWarning("Token refresh failed: invalid request");
            return ResultService<RefreshTokenResponse>.Fail("Access token and refresh token are required", ResultErrorCode.ValidationError);
        }

        try
        {
            var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(request.AccessToken);
            var email = principal?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Token refresh failed: could not extract email from token");
                return ResultService<RefreshTokenResponse>.Fail("Invalid token", ResultErrorCode.ValidationError);
            }

            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Token refresh failed: user not found for email {Email}", email);
                return ResultService<RefreshTokenResponse>.Fail("User not found", ResultErrorCode.ValidationError);
            }

            if (user.RefreshToken != request.RefreshToken)
            {
                _logger.LogWarning("Token refresh failed: refresh token mismatch for user {Email}", email);
                return ResultService<RefreshTokenResponse>.Fail("Invalid refresh token", ResultErrorCode.ValidationError);
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Token refresh failed: refresh token expired for user {Email}", email);
                return ResultService<RefreshTokenResponse>.Fail("Refresh token expired", ResultErrorCode.ValidationError);
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            _logger.LogInformation("Token refreshed successfully for user: {Email}", email);
            return ResultService<RefreshTokenResponse>.Ok(response, "Token refreshed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh failed with exception");
            return ResultService<RefreshTokenResponse>.Fail("Invalid token", ResultErrorCode.ValidationError);
        }
    }
}
