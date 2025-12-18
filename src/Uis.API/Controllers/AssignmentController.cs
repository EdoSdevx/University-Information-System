// Controllers/AssignmentController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uis.API.DTOs;
using Uis.API.Services;
using Uis.API.Services.Interfaces;

namespace Uis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly IFileService _fileService;
    private readonly ILogger<AssignmentController> _logger;

    public AssignmentController(IAssignmentService assignmentService, ILogger<AssignmentController> logger, IFileService fileService)
    {
        _assignmentService = assignmentService;
        _fileService = fileService;
        _logger = logger;

    }

    [HttpGet("course/{courseInstanceId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<List<AssignmentResponse>>>> GetCourseAssignments(int courseInstanceId)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.GetTeacherAssignmentsAsync(teacherId, courseInstanceId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<AssignmentResponse>>> CreateAssignment([FromBody] CreateAssignmentRequest request)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.CreateAssignmentAsync(teacherId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return CreatedAtAction(nameof(GetAssignmentSubmissions), new { assignmentId = result.Data?.Id }, result);
    }

    [HttpPut("{assignmentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<AssignmentResponse>>> UpdateAssignment(int assignmentId, [FromBody] UpdateAssignmentRequest request)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.UpdateAssignmentAsync(assignmentId, teacherId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete("{assignmentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService>> DeleteAssignment(int assignmentId)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.DeleteAssignmentAsync(assignmentId, teacherId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("{assignmentId}/submissions")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService<List<SubmissionResponse>>>> GetAssignmentSubmissions(int assignmentId)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.GetAssignmentSubmissionsAsync(assignmentId, teacherId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost("submissions/{submissionId}/grade")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<ResultService>> GradeSubmission(int submissionId, [FromBody] GradeSubmissionRequest request)
    {
        var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (teacherIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var teacherId = int.Parse(teacherIdClaim);

        var result = await _assignmentService.GradeSubmissionAsync(submissionId, teacherId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

   
    [HttpGet("my-assignments")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ResultService<List<StudentAssignmentResponse>>>> GetMyAssignments()
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var studentId = int.Parse(studentIdClaim);

        var result = await _assignmentService.GetStudentAssignmentsAsync(studentId);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost("submit")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ResultService>> SubmitAssignment([FromForm] SubmitAssignmentRequest request)
    {
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (studentIdClaim == null)
        {
            return Unauthorized(new { message = "You are not authorized." });
        }

        var studentId = int.Parse(studentIdClaim);

        string? fileUrl = null;

        if (request.File != null)
        {
            var uploadResult = await _fileService.UploadFileAsync(request.File, "assignments");

            if (!uploadResult.Success)
            {
                return BadRequest(uploadResult);
            }

            fileUrl = uploadResult.Data;
        }

        var dto = new SubmitAssignmentDto
        {
            SubmissionText = request.SubmissionText!,
            FileUrl = fileUrl
        };

        var result = await _assignmentService.SubmitAssignmentAsync(studentId, request.AssignmentId, dto);

        if (!result.Success)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
}