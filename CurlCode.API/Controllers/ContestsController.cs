using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.Contests;
using CurlCode.Application.Services.Contests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContestsController : ControllerBase
{
    private readonly IContestService _contestService;
    private readonly ICurrentUserService _currentUserService;

    public ContestsController(IContestService contestService, ICurrentUserService currentUserService)
    {
        _contestService = contestService;
        _currentUserService = currentUserService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContestDetailDto>> GetById(int id)
    {
        var contest = await _contestService.GetContestByIdAsync(id);
        if (contest == null) return NotFound();
        return Ok(contest);
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<ContestDto>>> GetUpcoming()
    {
        var contests = await _contestService.GetUpcomingContestsAsync();
        return Ok(contests);
    }

    [HttpGet("running")]
    public async Task<ActionResult<IEnumerable<ContestDto>>> GetRunning()
    {
        var contests = await _contestService.GetRunningContestsAsync();
        return Ok(contests);
    }

    [HttpGet("past")]
    public async Task<ActionResult<IEnumerable<ContestDto>>> GetPast([FromQuery] int count = 10)
    {
        var contests = await _contestService.GetPastContestsAsync(count);
        return Ok(contests);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ContestDto>> Create([FromBody] CreateContestRequest request)
    {
        var contest = await _contestService.CreateContestAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = contest.Id }, contest);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ContestDto>> Update(int id, [FromBody] UpdateContestRequest request)
    {
        var contest = await _contestService.UpdateContestAsync(id, request);
        if (contest == null) return NotFound();
        return Ok(contest);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _contestService.DeleteContestAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/join")]
    [Authorize]
    public async Task<IActionResult> Join(int id)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            return Unauthorized();

        var result = await _contestService.JoinContestAsync(id, _currentUserService.UserId);
        if (!result) return Conflict("Already registered for this contest.");
        return Ok("Successfully joined the contest.");
    }

    [HttpGet("{id}/standings")]
    public async Task<ActionResult<IEnumerable<ContestStandingDto>>> GetStandings(int id)
    {
        var standings = await _contestService.GetStandingsAsync(id);
        return Ok(standings);
    }
}
