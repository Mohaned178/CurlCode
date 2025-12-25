using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.Submissions;
using CurlCode.Application.Services.Submissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _submissionService;
    private readonly ICurrentUserService _currentUserService;

    public SubmissionsController(
        ISubmissionService submissionService,
        ICurrentUserService currentUserService)
    {
        _submissionService = submissionService;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<ActionResult<SubmissionResultDto>> SubmitCode([FromBody] SubmitCodeRequest request)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var submission = await _submissionService.SubmitCodeAsync(request, _currentUserService.UserId);
        return Ok(submission);
    }

    [HttpGet("me")]
    public async Task<ActionResult<IEnumerable<SubmissionResultDto>>> GetMySubmissions()
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var submissions = await _submissionService.GetMySubmissionsAsync(_currentUserService.UserId);
        return Ok(submissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubmissionResultDto>> GetSubmission(int id)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var submission = await _submissionService.GetSubmissionByIdAsync(id, _currentUserService.UserId);
        if (submission == null)
        {
            return NotFound();
        }

        return Ok(submission);
    }
}






