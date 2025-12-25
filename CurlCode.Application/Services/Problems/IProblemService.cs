using CurlCode.Application.Common.Models;
using CurlCode.Application.DTOs.Problems;

namespace CurlCode.Application.Services.Problems;

public interface IProblemService
{
    Task<PagedResult<ProblemDto>> GetProblemsAsync(FilterParams filterParams);
    Task<ProblemDetailDto?> GetProblemByIdAsync(int id);
    Task<ProblemDetailDto> CreateProblemAsync(CreateProblemDto dto);
    Task<ProblemDetailDto> UpdateProblemAsync(int id, CreateProblemDto dto);
    Task DeleteProblemAsync(int id);
    Task ToggleLikeAsync(int problemId, string userId);
    Task ToggleDislikeAsync(int problemId, string userId);
    Task<bool> IsLikedByUserAsync(int problemId, string userId);
    Task<bool> IsDislikedByUserAsync(int problemId, string userId);
    Task AddCommentAsync(int problemId, string userId, string content);
    Task DeleteCommentAsync(int commentId, string userId);
    Task<IEnumerable<DTOs.Community.CommentDto>> GetCommentsAsync(int problemId);
    Task<ProblemDto?> GetRandomAsync();
    Task<List<ProblemDto>> GetMostLikedProblemsAsync(int count = 1);
    Task<IEnumerable<DTOs.Solutions.SolutionDto>> GetSolutionsByProblemIdAsync(int problemId, string? currentUserId);
}




