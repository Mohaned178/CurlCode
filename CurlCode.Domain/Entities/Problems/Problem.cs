using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Submissions;
using CurlCode.Domain.Enums;

namespace CurlCode.Domain.Entities.Problems;

public class Problem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Constraints { get; set; }
    public string? Examples { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    
    // Limits
    public int TimeLimitMs { get; set; } = 2000;
    public int MemoryLimitMb { get; set; } = 256;
    
    // Statistics
    public int TotalSubmissions { get; set; } = 0;
    public int AcceptedSubmissions { get; set; } = 0;
    public double AcceptanceRate => TotalSubmissions > 0 ? (double)AcceptedSubmissions / TotalSubmissions * 100 : 0;
    public int LikesCount { get; set; } = 0;
    public int DislikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;

    // Navigation properties
    public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    public ICollection<ProblemTopic> ProblemTopics { get; set; } = new List<ProblemTopic>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
    public ICollection<ProblemLike> Likes { get; set; } = new List<ProblemLike>();
    public ICollection<ProblemDislike> Dislikes { get; set; } = new List<ProblemDislike>();
    public ICollection<ProblemComment> Comments { get; set; } = new List<ProblemComment>();
   
}

