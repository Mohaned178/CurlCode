using CurlCode.Application.Common.Models;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.Services.Solutions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolutionsController : ControllerBase
{
    private readonly ISolutionService _solutionService;
    private readonly ICurrentUserService _currentUserService;

    public SolutionsController(
        ISolutionService solutionService,
        ICurrentUserService currentUserService)
    {
        _solutionService = solutionService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResult<SolutionDto>>> GetSolutions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = "newest")
    {
        var filterParams = new FilterParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            SortBy = sortBy
        };

        var solutions = await _solutionService.GetSolutionsAsync(
            filterParams,
            _currentUserService.UserId);

        return Ok(solutions);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<SolutionDto>> GetSolution(int id)
    {
        var solution = await _solutionService.GetSolutionByIdAsync(id, _currentUserService.UserId);
        if (solution == null)
        {
            return NotFound();
        }

        return Ok(solution);
    }

    [HttpGet("problem/{problemId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SolutionDto>>> GetSolutionsByProblem(int problemId)
    {
        var solutions = await _solutionService.GetSolutionsByProblemIdAsync(
            problemId,
            _currentUserService.UserId);

        return Ok(solutions);
    }

    [HttpGet("most-liked")]
    [Authorize]
    public async Task<ActionResult<List<SolutionDto>>> GetMostLikedSolutions([FromQuery] int count = 3)
    {
        var solutions = await _solutionService.GetMostLikedSolutionsAsync(count);
        return Ok(solutions);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SolutionDto>> CreateSolution([FromBody] CreateSolutionDto dto)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var solution = await _solutionService.CreateSolutionAsync(dto, _currentUserService.UserId);
        return CreatedAtAction(nameof(GetSolution), new { id = solution.Id }, solution);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<SolutionDto>> UpdateSolution(int id, [FromBody] CreateSolutionDto dto)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var solution = await _solutionService.UpdateSolutionAsync(id, dto, _currentUserService.UserId);
        return Ok(solution);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteSolution(int id)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        await _solutionService.DeleteSolutionAsync(id, _currentUserService.UserId);
        return NoContent();
    }

    [HttpPost("{id}/like")]
    [Authorize]
    public async Task<IActionResult> ToggleLike(int id)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        await _solutionService.ToggleLikeAsync(id, _currentUserService.UserId);
        return Ok();
    }

    [HttpPost("{id}/dislike")]
    [Authorize]
    public async Task<IActionResult> ToggleDislike(int id)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        await _solutionService.ToggleDislikeAsync(id, _currentUserService.UserId);
        return Ok();
    }

}




