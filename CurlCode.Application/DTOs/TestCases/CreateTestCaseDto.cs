using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.TestCases;

public class CreateTestCaseDto
{
    [Required(ErrorMessage = "Input is required")]
    [MinLength(1, ErrorMessage = "Input cannot be empty")]
    public string Input { get; set; } = string.Empty;

    [Required(ErrorMessage = "ExpectedOutput is required")]
    [MinLength(1, ErrorMessage = "ExpectedOutput cannot be empty")]
    public string ExpectedOutput { get; set; } = string.Empty;

    public bool IsHidden { get; set; } = true;
    
    [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number")]
    public int Order { get; set; }
}






