using CurlCode.Domain.Common;

namespace CurlCode.Domain.Entities.Identity;

public class UserProfile : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
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
    
    // Statistics
    public int TotalSolved { get; set; } = 0;
    public int EasySolved { get; set; } = 0;
    public int MediumSolved { get; set; } = 0;
    public int HardSolved { get; set; } = 0;
    public int TotalScore { get; set; } = 0;
    public int Rank { get; set; } = 0;
    
    // Navigation property
    public ApplicationUser User { get; set; } = null!;
}






