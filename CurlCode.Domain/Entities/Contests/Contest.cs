using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Enums;

namespace CurlCode.Domain.Entities.Contests;

public class Contest : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ContestStatus Status { get; set; } = ContestStatus.Upcoming;
    public bool IsPublic { get; set; } = true;
    
    // Navigation properties
    public ICollection<ContestProblem> ContestProblems { get; set; } = new List<ContestProblem>();
    public ICollection<ContestParticipant> Participants { get; set; } = new List<ContestParticipant>();
}
