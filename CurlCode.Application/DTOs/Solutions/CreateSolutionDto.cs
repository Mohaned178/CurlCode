using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Solutions;

public class CreateSolutionDto
{
    [Required(ErrorMessage = "ProblemId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ProblemId must be a positive number")]
    public int ProblemId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    [MinLength(10, ErrorMessage = "Content must be at least 10 characters")]
    public string Content { get; set; } = string.Empty;

    [StringLength(10000, ErrorMessage = "Code cannot exceed 10000 characters")]
    public string? Code { get; set; }
}






