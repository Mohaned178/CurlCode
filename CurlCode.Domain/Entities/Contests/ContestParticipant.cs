using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Domain.Entities.Contests;

public class ContestParticipant : BaseEntity
{
    public int ContestId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int Score { get; set; } = 0;
    public int SolvedCount { get; set; } = 0;
    public int Rank { get; set; } = 0;
    public int Penalty { get; set; } = 0; // Time penalty in minutes
    
    // Navigation properties
    public Contest Contest { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
