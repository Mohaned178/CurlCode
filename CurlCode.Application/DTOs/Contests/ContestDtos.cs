using CurlCode.Domain.Enums;

namespace CurlCode.Application.DTOs.Contests;

public class ContestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ContestStatus Status { get; set; }
    public bool IsPublic { get; set; }
    public int ParticipantCount { get; set; }
    public int ProblemCount { get; set; }
}

public class ContestDetailDto : ContestDto
{
    public IEnumerable<ContestProblemDto> Problems { get; set; } = new List<ContestProblemDto>();
}

public class ContestProblemDto
{
    public int ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Points { get; set; }
    public DifficultyLevel Difficulty { get; set; }
}

public class ContestStandingDto
{
    public int Rank { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int Score { get; set; }
    public int SolvedCount { get; set; }
    public int Penalty { get; set; }
}

public class CreateContestRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsPublic { get; set; } = true;
    public List<int> ProblemIds { get; set; } = new();
}

public class UpdateContestRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IsPublic { get; set; }
}
