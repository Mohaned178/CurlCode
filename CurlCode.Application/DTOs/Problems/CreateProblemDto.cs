using CurlCode.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Problems;

public class CreateProblemDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    public string Description { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Constraints cannot exceed 2000 characters")]
    public string? Constraints { get; set; }
    
    [StringLength(2000, ErrorMessage = "Examples cannot exceed 2000 characters")]
    public string? Examples { get; set; }

    [Required(ErrorMessage = "Difficulty is required")]
    public DifficultyLevel Difficulty { get; set; }

    [Range(100, 10000, ErrorMessage = "TimeLimitMs must be between 100 and 10000 milliseconds")]
    public int TimeLimitMs { get; set; } = 2000;
    
    [Range(64, 1024, ErrorMessage = "MemoryLimitMb must be between 64 and 1024 MB")]
    public int MemoryLimitMb { get; set; } = 256;

    public List<int> TopicIds { get; set; } = new();
}






