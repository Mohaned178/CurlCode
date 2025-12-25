using CurlCode.Domain.Common;

namespace CurlCode.Domain.Entities.Problems;

public class Topic : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Navigation property
    public ICollection<ProblemTopic> ProblemTopics { get; set; } = new List<ProblemTopic>();
}






