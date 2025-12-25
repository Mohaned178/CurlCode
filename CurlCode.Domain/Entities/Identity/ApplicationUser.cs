using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Submissions;
using Microsoft.AspNetCore.Identity;

namespace CurlCode.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsAdmin { get; set; } = false;
    
    // Navigation properties
    public UserProfile? Profile { get; set; }
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<SolutionLike> SolutionLikes { get; set; } = new List<SolutionLike>();
}

