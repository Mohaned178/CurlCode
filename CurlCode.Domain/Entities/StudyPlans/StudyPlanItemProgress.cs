using CurlCode.Domain.Common;

namespace CurlCode.Domain.Entities.StudyPlans;

public class StudyPlanItemProgress : BaseEntity
{
    public int StudyPlanProgressId { get; set; }
    public int StudyPlanItemId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public StudyPlanProgress StudyPlanProgress { get; set; } = null!;
    public StudyPlanItem StudyPlanItem { get; set; } = null!;
}

