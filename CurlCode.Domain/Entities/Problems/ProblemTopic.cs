namespace CurlCode.Domain.Entities.Problems;

public class ProblemTopic
{
    public int ProblemId { get; set; }
    public int TopicId { get; set; }
    
    // Navigation properties
    public Problem Problem { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
}






