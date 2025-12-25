using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Problems;

public class CommentRequest
{
    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(2000, ErrorMessage = "Comment cannot exceed 2000 characters")]
    [MinLength(1, ErrorMessage = "Comment cannot be empty")]
    public string Content { get; set; } = string.Empty;
}
