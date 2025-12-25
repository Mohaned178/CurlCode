using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Domain.Entities.Community;

public class Solution : BaseEntity
{
    public int ProblemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int LikesCount { get; set; } = 0;
    public int DislikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
    
    // Navigation properties
    public Problem Problem { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<SolutionLike> Likes { get; set; } = new List<SolutionLike>();
    public ICollection<SolutionDislike> Dislikes { get; set; } = new List<SolutionDislike>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}






