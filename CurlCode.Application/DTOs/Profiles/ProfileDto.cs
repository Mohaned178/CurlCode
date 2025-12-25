namespace CurlCode.Application.DTOs.Profiles;

public class ProfileDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Country { get; set; }
    public string? University { get; set; }
    public string? Work { get; set; }
    public string? Major { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int TotalSolved { get; set; }
    public int EasySolved { get; set; }
    public int MediumSolved { get; set; }
    public int HardSolved { get; set; }
    public int TotalScore { get; set; }
    public int Rank { get; set; }
    public DateTime CreatedAt { get; set; }
}






