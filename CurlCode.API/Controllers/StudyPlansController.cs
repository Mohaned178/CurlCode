using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.StudyPlans;
using CurlCode.Application.Services.StudyPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudyPlansController : ControllerBase
{
    private readonly IStudyPlanService _studyPlanService;
    private readonly ICurrentUserService _currentUserService;

    public StudyPlansController(IStudyPlanService studyPlanService, ICurrentUserService currentUserService)
    {
        _studyPlanService = studyPlanService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyPlanDto>>> GetActiveStudyPlans()
    {
        var studyPlans = await _studyPlanService.GetActiveStudyPlansAsync();
        return Ok(studyPlans);
    }

    [HttpGet("my-custom")]
    public async Task<ActionResult<List<StudyPlanDto>>> GetMyCustomStudyPlans()
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var studyPlans = await _studyPlanService.GetUserCustomStudyPlansAsync(userId);
        return Ok(studyPlans);
    }

    [HttpPost("create")]
        public async Task<ActionResult<StudyPlanDto>> CreateCustomStudyPlan([FromBody] CreateStudyPlanDto dto)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var studyPlan = await _studyPlanService.CreateCustomStudyPlanAsync(dto, userId);
        return CreatedAtAction(nameof(GetStudyPlan), new { id = studyPlan.Id }, studyPlan);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudyPlanDto>> GetStudyPlan(int id)
    {
        var studyPlan = await _studyPlanService.GetByIdAsync(id);
        if (studyPlan == null)
            return NotFound();

        return Ok(studyPlan);
    }

    [HttpGet("my-progress")]
    public async Task<ActionResult<List<StudyPlanProgressDto>>> GetMyStudyPlans()
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var progresses = await _studyPlanService.GetUserStudyPlansAsync(userId);
        return Ok(progresses);
    }

    [HttpGet("{id}/progress")]
    public async Task<ActionResult<StudyPlanProgressDto>> GetMyProgress(int id)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var progress = await _studyPlanService.GetUserProgressAsync(id, userId);
        if (progress == null)
            return NotFound();

        return Ok(progress);
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<StudyPlanProgressDto>> StartStudyPlan(int id)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var progress = await _studyPlanService.StartStudyPlanAsync(id, userId);
        return Ok(progress);
    }

    [HttpPost("{studyPlanId}/problems/{problemId}/complete")]
    public async Task<IActionResult> MarkProblemCompleted(int studyPlanId, int problemId)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _studyPlanService.MarkProblemCompletedAsync(studyPlanId, problemId, userId);
        if (!result)
            return BadRequest("Failed to mark problem as completed");

        return Ok(new { message = "Problem marked as completed" });
    }

    [HttpPost("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudyPlanDto>> CreateStudyPlan([FromBody] CreateStudyPlanDto dto)
    {
        var studyPlan = await _studyPlanService.CreateStudyPlanAsync(dto);
        return CreatedAtAction(nameof(GetStudyPlan), new { id = studyPlan.Id }, studyPlan);
    }

    [HttpPut("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudyPlanDto>> UpdateStudyPlan(int id, [FromBody] CreateStudyPlanDto dto)
    {
        var studyPlan = await _studyPlanService.UpdateStudyPlanAsync(id, dto);
        return Ok(studyPlan);
    }

    [HttpDelete("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudyPlan(int id)
    {
        await _studyPlanService.DeleteStudyPlanAsync(id);
        return NoContent();
    }
}

