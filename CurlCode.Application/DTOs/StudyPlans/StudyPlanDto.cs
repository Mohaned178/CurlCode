using CurlCode.Domain.Enums;

namespace CurlCode.Application.DTOs.StudyPlans;

public class StudyPlanDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int DurationDays { get; set; }
    public int TotalProblems { get; set; }
    public bool IsActive { get; set; }
    public bool IsCustom { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<StudyPlanItemDto> Items { get; set; } = new();
}

public class StudyPlanItemDto
{
    public int Id { get; set; }
    public int ProblemId { get; set; }
    public string ProblemTitle { get; set; } = string.Empty;
    public DifficultyLevel ProblemDifficulty { get; set; }
    public int DayNumber { get; set; }
    public int Order { get; set; }
}

