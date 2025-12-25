using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.TestCases;
using CurlCode.Application.Services.Problems;
using CurlCode.Domain.Entities.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/problems/{problemId}/[controller]")]
public class TestCasesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TestCasesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TestCaseDto>>> GetTestCases(int problemId)
    {
        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(problemId);
        if (problem == null)
        {
            return NotFound("Problem not found");
        }

        var testCases = problem.TestCases.OrderBy(tc => tc.Order).ToList();
        var testCaseDtos = testCases.Select(tc => new TestCaseDto
        {
            Id = tc.Id,
            ProblemId = tc.ProblemId,
            Input = tc.Input,
            ExpectedOutput = tc.ExpectedOutput,
            IsHidden = tc.IsHidden,
            Order = tc.Order
        });

        return Ok(testCaseDtos);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TestCaseDto>> CreateTestCase(int problemId, [FromBody] CreateTestCaseDto dto)
    {
        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(problemId);
        if (problem == null)
        {
            return NotFound("Problem not found");
        }

        var testCase = new TestCase
        {
            ProblemId = problemId,
            Input = dto.Input,
            ExpectedOutput = dto.ExpectedOutput,
            IsHidden = dto.IsHidden,
            Order = dto.Order
        };

        problem.TestCases.Add(testCase);
        await _unitOfWork.SaveChangesAsync();

        var testCaseDto = new TestCaseDto
        {
            Id = testCase.Id,
            ProblemId = testCase.ProblemId,
            Input = testCase.Input,
            ExpectedOutput = testCase.ExpectedOutput,
            IsHidden = testCase.IsHidden,
            Order = testCase.Order
        };

        return CreatedAtAction(nameof(GetTestCases), new { problemId }, testCaseDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTestCase(int problemId, int id)
    {
        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(problemId);
        if (problem == null)
        {
            return NotFound("Problem not found");
        }

        var testCaseToDelete = problem.TestCases.FirstOrDefault(tc => tc.Id == id);
        if (testCaseToDelete == null)
        {
            return NotFound("Test case not found");
        }

        problem.TestCases.Remove(testCaseToDelete);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}

