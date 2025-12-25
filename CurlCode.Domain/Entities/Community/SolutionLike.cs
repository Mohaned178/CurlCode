using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Domain.Entities.Community;

public class SolutionLike : BaseEntity
{
    public int SolutionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Navigation properties
    public Solution Solution { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}






