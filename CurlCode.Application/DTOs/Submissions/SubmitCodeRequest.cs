using CurlCode.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Submissions;

public class SubmitCodeRequest
{
    [Required(ErrorMessage = "ProblemId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ProblemId must be a positive number")]
    public int ProblemId { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [MinLength(1, ErrorMessage = "Code cannot be empty")]
    [MaxLength(50000, ErrorMessage = "Code cannot exceed 50000 characters")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Language is required")]
    public ProgrammingLanguage Language { get; set; }
}






