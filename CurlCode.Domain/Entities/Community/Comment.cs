using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Domain.Entities.Community;

public class Comment : BaseEntity
{
    public int SolutionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    // Navigation properties
    public Solution Solution { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
}


