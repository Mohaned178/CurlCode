using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Domain.Entities.StudyPlans;

public class StudyPlanProgress : BaseEntity
{
    public int StudyPlanId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CompletedProblems { get; set; } = 0;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted => CompletedAt != null;

    // Navigation properties
    public StudyPlan StudyPlan { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<StudyPlanItemProgress> ItemProgresses { get; set; } = new List<StudyPlanItemProgress>();
}

