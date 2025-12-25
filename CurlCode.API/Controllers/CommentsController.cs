using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.Community;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.Services.Problems;
using CurlCode.Application.Services.Solutions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IProblemService _problemService;
    private readonly ISolutionService _solutionService;
    private readonly ICurrentUserService _currentUserService;

    public CommentsController(
        IProblemService problemService,
        ISolutionService solutionService,
        ICurrentUserService currentUserService)
    {
        _problemService = problemService;
        _solutionService = solutionService;
        _currentUserService = currentUserService;
    }

    [HttpGet("problems/{problemId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetProblemComments(int problemId)
    {
        var comments = await _problemService.GetCommentsAsync(problemId);
        return Ok(comments);
    }

    [HttpPost("problems/{problemId}")]
    [Authorize]
    public async Task<IActionResult> AddProblemComment(int problemId, [FromBody] CommentRequest request)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId)) return Unauthorized();
        await _problemService.AddCommentAsync(problemId, _currentUserService.UserId, request.Content);
        return Ok();
    }

    [HttpDelete("problems/{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteProblemComment(int commentId)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId)) return Unauthorized();
        await _problemService.DeleteCommentAsync(commentId, _currentUserService.UserId);
        return NoContent();
    }

    [HttpGet("solutions/{solutionId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetSolutionComments(int solutionId)
    {
        var comments = await _solutionService.GetCommentsAsync(solutionId);
        return Ok(comments);
    }

    [HttpPost("solutions/{solutionId}")]
    [Authorize]
    public async Task<IActionResult> AddSolutionComment(int solutionId, [FromBody] CommentRequest request)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId)) return Unauthorized();
        await _solutionService.AddCommentAsync(solutionId, request.Content, _currentUserService.UserId);
        return Ok();
    }

    [HttpDelete("solutions/{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteSolutionComment(int commentId)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId)) return Unauthorized();
        await _solutionService.DeleteCommentAsync(commentId, _currentUserService.UserId);
        return NoContent();
    }
}

