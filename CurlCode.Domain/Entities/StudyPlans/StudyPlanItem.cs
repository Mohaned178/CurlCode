using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Domain.Entities.StudyPlans;

public class StudyPlanItem : BaseEntity
{
    public int StudyPlanId { get; set; }
    public int ProblemId { get; set; }
    public int DayNumber { get; set; } // Which day in the study plan this problem should be solved
    public int Order { get; set; } // Order within the day

    // Navigation properties
    public StudyPlan StudyPlan { get; set; } = null!;
    public Problem Problem { get; set; } = null!;
}

