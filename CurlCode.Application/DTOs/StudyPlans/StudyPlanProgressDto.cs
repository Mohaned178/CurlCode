namespace CurlCode.Application.DTOs.StudyPlans;

public class StudyPlanProgressDto
{
    public int Id { get; set; }
    public int StudyPlanId { get; set; }
    public string StudyPlanTitle { get; set; } = string.Empty;
    public int CompletedProblems { get; set; }
    public int TotalProblems { get; set; }
    public double ProgressPercentage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public List<StudyPlanItemProgressDto> ItemProgresses { get; set; } = new();
}

public class StudyPlanItemProgressDto
{
    public int Id { get; set; }
    public int StudyPlanItemId { get; set; }
    public int ProblemId { get; set; }
    public string ProblemTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}

