using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Domain.Entities.Contests;

public class ContestProblem : BaseEntity
{
    public int ContestId { get; set; }
    public int ProblemId { get; set; }
    public int Order { get; set; } // Display order (A, B, C...)
    public int Points { get; set; } = 100; // Points for solving this problem
    
    // Navigation properties
    public Contest Contest { get; set; } = null!;
    public Problem Problem { get; set; } = null!;
}
