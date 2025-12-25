using System.ComponentModel.DataAnnotations;
using CurlCode.Domain.Enums;

namespace CurlCode.Application.DTOs.StudyPlans;

public class CreateStudyPlanDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Difficulty is required")]
    public DifficultyLevel Difficulty { get; set; }
    
    [Range(1, 365, ErrorMessage = "DurationDays must be between 1 and 365")]
    public int DurationDays { get; set; }
    
    [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
    public DateTime? Deadline { get; set; }
    
    public List<StudyPlanItemCreateDto> Items { get; set; } = new();
}

public class StudyPlanItemCreateDto
{
    [Required(ErrorMessage = "ProblemId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ProblemId must be a positive number")]
    public int ProblemId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "DayNumber must be a positive number")]
    public int DayNumber { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number")]
    public int Order { get; set; }
}

