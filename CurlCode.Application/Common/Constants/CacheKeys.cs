namespace CurlCode.Application.Common.Constants;

public static class CacheKeys
{
    public const string ProblemsListPrefix = "problems_list_";
    public const string ProblemDetailPrefix = "problem_detail_";
    public const string TopicsList = "topics_list";
    
    public static string GetProblemDetailKey(int id) => $"{ProblemDetailPrefix}{id}";
    public static string GetProblemsListKey(object filterParams) => $"{ProblemsListPrefix}{filterParams.GetHashCode()}";
}
