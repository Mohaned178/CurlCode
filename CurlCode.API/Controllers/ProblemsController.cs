using CurlCode.Application.Common.Models;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Application.Services.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProblemsController : ControllerBase
{
    private readonly IProblemService _problemService;

    public ProblemsController(IProblemService problemService)
    {
        _problemService = problemService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProblemDto>>> GetProblems(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Domain.Enums.DifficultyLevel? difficulty = null,
        [FromQuery] int? topicId = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] string? sortOrder = "desc")
    {
        var filterParams = new FilterParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Difficulty = difficulty,
            TopicId = topicId,
            SortBy = sortBy,
            SortOrder = sortOrder
        };
        var result = await _problemService.GetProblemsAsync(filterParams);

        return Ok(result);
    }

    [HttpGet("most-liked")]
    [Authorize]
    public async Task<ActionResult<List<ProblemDto>>> GetMostLikedProblems([FromQuery] int count = 3)
    {
        var problems = await _problemService.GetMostLikedProblemsAsync(count);
        return Ok(problems);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ProblemDetailDto>> GetProblem(int id)
    {
        var problem = await _problemService.GetProblemByIdAsync(id);
        if (problem == null) return NotFound();
        return Ok(problem);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProblemDetailDto>> CreateProblem([FromBody] CreateProblemDto dto)
    {
        var problem = await _problemService.CreateProblemAsync(dto);
        

        
        return CreatedAtAction(nameof(GetProblem), new { id = problem.Id }, problem);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProblemDetailDto>> UpdateProblem(int id, [FromBody] CreateProblemDto dto)
    {
        var problem = await _problemService.UpdateProblemAsync(id, dto);
        

        
        return Ok(problem);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProblem(int id)
    {
        await _problemService.DeleteProblemAsync(id);
        

        
        return NoContent();
    }

    [HttpPost("{id}/like")]
    [Authorize]
    public async Task<IActionResult> ToggleLike(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        await _problemService.ToggleLikeAsync(id, userId);
        return Ok();
    }

    [HttpPost("{id}/dislike")]
    [Authorize]
    public async Task<IActionResult> ToggleDislike(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        await _problemService.ToggleDislikeAsync(id, userId);
        return Ok();
    }

    [HttpGet("{id}/solutions")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SolutionDto>>> GetSolutionsByProblem(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var solutions = await _problemService.GetSolutionsByProblemIdAsync(id, userId);
        return Ok(solutions);
    }

    [HttpGet("random")]
    public async Task<ActionResult<ProblemDto>> GetRandomProblem()
    {
        var problem = await _problemService.GetRandomAsync();
        if (problem == null) return NotFound();
        return Ok(problem);
    }
}




