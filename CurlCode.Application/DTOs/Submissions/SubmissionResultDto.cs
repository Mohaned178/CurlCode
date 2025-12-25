using CurlCode.Domain.Enums;

namespace CurlCode.Application.DTOs.Submissions;

public class SubmissionResultDto
{
    public int Id { get; set; }
    public int ProblemId { get; set; }
    public string ProblemTitle { get; set; } = string.Empty;
    public ProgrammingLanguage Language { get; set; }
    public SubmissionStatus Status { get; set; }
    public int? ExecutionTimeMs { get; set; }
    public int? MemoryUsedMb { get; set; }
    public string? ErrorMessage { get; set; }
    public int? TestCasesPassed { get; set; }
    public int? TotalTestCases { get; set; }
    public DateTime CreatedAt { get; set; }
}






