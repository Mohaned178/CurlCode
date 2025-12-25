using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Enums;

namespace CurlCode.Domain.Entities.Submissions;

public class Submission : BaseEntity
{
    public int ProblemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ProgrammingLanguage Language { get; set; }
    public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
    
    // Execution results
    public int? ExecutionTimeMs { get; set; }
    public int? MemoryUsedMb { get; set; }
    public string? ErrorMessage { get; set; }
    public int? TestCasesPassed { get; set; }
    public int? TotalTestCases { get; set; }
    
    // Navigation properties
    public Problem Problem { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}






