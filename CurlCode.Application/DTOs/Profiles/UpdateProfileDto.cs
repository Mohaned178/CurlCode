using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Profiles;

public class UpdateProfileDto
{
    
    [StringLength(100, ErrorMessage = "FirstName cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "LastName cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    public string? Bio { get; set; }
    
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(500, ErrorMessage = "ProfileImageUrl cannot exceed 500 characters")]
    public string? ProfileImageUrl { get; set; }
    
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(200, ErrorMessage = "GitHubUrl cannot exceed 200 characters")]
    public string? GitHubUrl { get; set; }
    
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(200, ErrorMessage = "LinkedInUrl cannot exceed 200 characters")]
    public string? LinkedInUrl { get; set; }
    
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(200, ErrorMessage = "WebsiteUrl cannot exceed 200 characters")]
    public string? WebsiteUrl { get; set; }
    
    [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string? Country { get; set; }
    
    [StringLength(200, ErrorMessage = "University cannot exceed 200 characters")]
    public string? University { get; set; }
    
    [StringLength(200, ErrorMessage = "Work cannot exceed 200 characters")]
    public string? Work { get; set; }
    
    [StringLength(100, ErrorMessage = "Major cannot exceed 100 characters")]
    public string? Major { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateTime? DateOfBirth { get; set; }
}





