using CurlCode.Domain.Enums;

namespace CurlCode.Application.DTOs.Problems;

public class ProblemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int TimeLimitMs { get; set; }
    public int MemoryLimitMb { get; set; }
    public int TotalSubmissions { get; set; }
    public int AcceptedSubmissions { get; set; }
    public double AcceptanceRate { get; set; }
    public List<string> Topics { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}






