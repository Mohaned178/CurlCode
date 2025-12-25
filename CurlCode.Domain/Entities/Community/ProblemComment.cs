using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Domain.Entities.Community;

public class ProblemComment : BaseEntity
{
    public int ProblemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    // Navigation properties
    public Problem Problem { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}

