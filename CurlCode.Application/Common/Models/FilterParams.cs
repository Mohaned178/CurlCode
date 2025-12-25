using CurlCode.Domain.Enums;

namespace CurlCode.Application.Common.Models;

public class FilterParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public DifficultyLevel? Difficulty { get; set; }
    public int? TopicId { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}






