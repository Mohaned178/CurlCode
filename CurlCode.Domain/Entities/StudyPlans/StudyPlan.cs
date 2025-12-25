using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Enums;

namespace CurlCode.Domain.Entities.StudyPlans;

public class StudyPlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int DurationDays { get; set; }
    public int TotalProblems { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsCustom { get; set; } = false; // User-created plan
    public string? UserId { get; set; } // Creator user ID for custom plans
    public DateTime? Deadline { get; set; } // Deadline for custom plans

    // Navigation properties
    public ApplicationUser? User { get; set; }
    public ICollection<StudyPlanItem> Items { get; set; } = new List<StudyPlanItem>();
    public ICollection<StudyPlanProgress> Progresses { get; set; } = new List<StudyPlanProgress>();
}

