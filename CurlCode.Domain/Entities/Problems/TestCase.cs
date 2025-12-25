using CurlCode.Domain.Common;

namespace CurlCode.Domain.Entities.Problems;

public class TestCase : BaseEntity
{
    public int ProblemId { get; set; }
    public string Input { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
    public bool IsHidden { get; set; } = true;
    public int Order { get; set; }
    
    // Navigation property
    public Problem Problem { get; set; } = null!;
}






