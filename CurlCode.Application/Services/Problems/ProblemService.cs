using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Common.Models;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Community;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Domain.Entities.Problems;
using System.Linq;

using CurlCode.Application.Common.Constants;
using CurlCode.Application.Contracts.Infrastructure;

namespace CurlCode.Application.Services.Problems;

public class ProblemService : IProblemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public ProblemService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<PagedResult<ProblemDto>> GetProblemsAsync(FilterParams filterParams)
    {
        var cacheKey = CacheKeys.GetProblemsListKey(filterParams);
        var cached = await _cacheService.GetAsync<PagedResult<ProblemDto>>(cacheKey);
        if (cached != null) return cached;

        var problems = await _unitOfWork.Problems.GetPagedAsync(filterParams);
        
        var problemDtos = _mapper.Map<List<ProblemDto>>(problems.Items);
        
        var result = new PagedResult<ProblemDto>
        {
            Items = problemDtos,
            TotalCount = problems.TotalCount,
            PageNumber = problems.PageNumber,
            PageSize = problems.PageSize
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));
        return result;
    }

    public async Task<ProblemDetailDto?> GetProblemByIdAsync(int id)
    {
        var cacheKey = CacheKeys.GetProblemDetailKey(id);
        var cached = await _cacheService.GetAsync<ProblemDetailDto>(cacheKey);
        if (cached != null) return cached;

        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(id);
        if (problem == null)
        {
            return null;
        }

        var result = _mapper.Map<ProblemDetailDto>(problem);
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));
        return result;
    }

    public async Task<ProblemDetailDto> CreateProblemAsync(CreateProblemDto dto)
    {
        var problem = _mapper.Map<Problem>(dto);
        
        await _unitOfWork.Problems.AddAsync(problem);
        
        // Add topics
        if (dto.TopicIds.Any())
        {
            foreach (var topicId in dto.TopicIds)
            {
                var topic = await _unitOfWork.Topics.GetByIdAsync(topicId);
                if (topic != null)
                {
                    problem.ProblemTopics.Add(new ProblemTopic
                    {
                        ProblemId = problem.Id,
                        TopicId = topicId
                    });
                }
            }
        }
        
        
        await _unitOfWork.SaveChangesAsync();
        
        await _cacheService.RemoveByPrefixAsync(CacheKeys.ProblemsListPrefix);

        var createdProblem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(problem.Id);
        return _mapper.Map<ProblemDetailDto>(createdProblem!);
    }

    public async Task<ProblemDetailDto> UpdateProblemAsync(int id, CreateProblemDto dto)
    {
        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(id);
        if (problem == null)
        {
            throw new NotFoundException(nameof(Problem), id);
        }

        problem.Title = dto.Title;
        problem.Description = dto.Description;
        problem.Constraints = dto.Constraints;
        problem.Examples = dto.Examples;
        problem.Difficulty = dto.Difficulty;
        problem.TimeLimitMs = dto.TimeLimitMs;
        problem.MemoryLimitMb = dto.MemoryLimitMb;
        problem.UpdatedAt = DateTime.UtcNow;

        // Update topics
        problem.ProblemTopics.Clear();
        if (dto.TopicIds.Any())
        {
            foreach (var topicId in dto.TopicIds)
            {
                var topic = await _unitOfWork.Topics.GetByIdAsync(topicId);
                if (topic != null)
                {
                    problem.ProblemTopics.Add(new ProblemTopic
                    {
                        ProblemId = problem.Id,
                        TopicId = topicId
                    });
                }
            }
        }

        await _unitOfWork.Problems.UpdateAsync(problem);
        await _unitOfWork.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.GetProblemDetailKey(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.ProblemsListPrefix);

        var updatedProblem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(id);
        return _mapper.Map<ProblemDetailDto>(updatedProblem!);
    }

    public async Task DeleteProblemAsync(int id)
    {
        var problem = await _unitOfWork.Problems.GetByIdAsync(id);
        if (problem == null)
        {
            throw new NotFoundException(nameof(Problem), id);
        }

        await _unitOfWork.Problems.DeleteAsync(problem);
        await _unitOfWork.SaveChangesAsync();
        
        await _cacheService.RemoveAsync(CacheKeys.GetProblemDetailKey(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.ProblemsListPrefix);
    }

    public async Task ToggleLikeAsync(int problemId, string userId)
    {
        await _unitOfWork.Problems.ToggleLikeAsync(problemId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ToggleDislikeAsync(int problemId, string userId)
    {
        await _unitOfWork.Problems.ToggleDislikeAsync(problemId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> IsLikedByUserAsync(int problemId, string userId)
    {
        return await _unitOfWork.Problems.IsLikedByUserAsync(problemId, userId);
    }

    public async Task<bool> IsDislikedByUserAsync(int problemId, string userId)
    {
        return await _unitOfWork.Problems.IsDislikedByUserAsync(problemId, userId);
    }

    public async Task AddCommentAsync(int problemId, string userId, string content)
    {
        await _unitOfWork.Problems.AddCommentAsync(problemId, userId, content);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, string userId)
    {
        await _unitOfWork.Problems.DeleteCommentAsync(commentId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(int problemId)
    {
        var comments = await _unitOfWork.Problems.GetCommentsAsync(problemId);
        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserName = c.User?.UserName
        });
    }

    public async Task<ProblemDto?> GetRandomAsync()
    {
        var problem = await _unitOfWork.Problems.GetRandomAsync();
        return problem == null ? null : _mapper.Map<ProblemDto>(problem);
    }

    public async Task<List<ProblemDto>> GetMostLikedProblemsAsync(int count = 1)
    {
        var problems = await _unitOfWork.Problems.GetMostLikedProblemsAsync(count);
        return _mapper.Map<List<ProblemDto>>(problems);
    }

    public async Task<IEnumerable<SolutionDto>> GetSolutionsByProblemIdAsync(int problemId, string? currentUserId)
    {
        // Verify problem exists
        var problem = await _unitOfWork.Problems.GetByIdAsync(problemId);
        if (problem == null)
        {
            throw new NotFoundException(nameof(Problem), problemId);
        }

        var solutions = await _unitOfWork.Solutions.GetByProblemIdAsync(problemId, currentUserId);
        var solutionDtos = _mapper.Map<List<SolutionDto>>(solutions);
        
        if (!string.IsNullOrEmpty(currentUserId))
        {
            foreach (var dto in solutionDtos)
            {
                dto.IsLikedByCurrentUser = await _unitOfWork.Solutions.IsLikedByUserAsync(dto.Id, currentUserId);
            }
        }
        
        return solutionDtos;
    }
}




