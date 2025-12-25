using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Common.Models;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Community;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Problems;
using System.Linq;

namespace CurlCode.Application.Services.Solutions;

public class SolutionService : ISolutionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SolutionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<SolutionDto>> GetSolutionsAsync(FilterParams filterParams, string? currentUserId)
    {
        var solutions = await _unitOfWork.Solutions.GetPagedAsync(filterParams, currentUserId);
        var solutionDtos = _mapper.Map<List<SolutionDto>>(solutions.Items);
        
        if (!string.IsNullOrEmpty(currentUserId))
        {
            foreach (var dto in solutionDtos)
            {
                dto.IsLikedByCurrentUser = await _unitOfWork.Solutions.IsLikedByUserAsync(dto.Id, currentUserId);
            }
        }
        
        return new PagedResult<SolutionDto>
        {
            Items = solutionDtos,
            TotalCount = solutions.TotalCount,
            PageNumber = solutions.PageNumber,
            PageSize = solutions.PageSize
        };
    }

    public async Task<SolutionDto?> GetSolutionByIdAsync(int id, string? currentUserId)
    {
        var solution = await _unitOfWork.Solutions.GetByIdWithDetailsAsync(id, currentUserId);
        if (solution == null)
        {
            return null;
        }

        var dto = _mapper.Map<SolutionDto>(solution);
        if (!string.IsNullOrEmpty(currentUserId))
        {
            dto.IsLikedByCurrentUser = await _unitOfWork.Solutions.IsLikedByUserAsync(id, currentUserId);
        }
        
        return dto;
    }

    public async Task<IEnumerable<SolutionDto>> GetSolutionsByProblemIdAsync(int problemId, string? currentUserId)
    {
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

    public async Task<SolutionDto> CreateSolutionAsync(CreateSolutionDto dto, string userId)
    {
        var problem = await _unitOfWork.Problems.GetByIdAsync(dto.ProblemId);
        if (problem == null)
        {
            throw new NotFoundException(nameof(Problem), dto.ProblemId);
        }

        var solution = _mapper.Map<Solution>(dto);
        solution.UserId = userId;

        await _unitOfWork.Solutions.AddAsync(solution);
        await _unitOfWork.SaveChangesAsync();

        var createdSolution = await _unitOfWork.Solutions.GetByIdWithDetailsAsync(solution.Id, userId);
        return _mapper.Map<SolutionDto>(createdSolution!);
    }

    public async Task<SolutionDto> UpdateSolutionAsync(int id, CreateSolutionDto dto, string userId)
    {
        var solution = await _unitOfWork.Solutions.GetByIdAsync(id);
        if (solution == null)
        {
            throw new NotFoundException(nameof(Solution), id);
        }

        if (solution.UserId != userId)
        {
            throw new Common.Exceptions.ValidationException("You can only update your own solutions.");
        }

        solution.Title = dto.Title;
        solution.Content = dto.Content;
        solution.Code = dto.Code;
        solution.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Solutions.UpdateAsync(solution);
        await _unitOfWork.SaveChangesAsync();

        var updatedSolution = await _unitOfWork.Solutions.GetByIdWithDetailsAsync(id, userId);
        return _mapper.Map<SolutionDto>(updatedSolution!);
    }

    public async Task DeleteSolutionAsync(int id, string userId)
    {
        var solution = await _unitOfWork.Solutions.GetByIdAsync(id);
        if (solution == null)
        {
            throw new NotFoundException(nameof(Solution), id);
        }

        if (solution.UserId != userId)
        {
            throw new Common.Exceptions.ValidationException("You can only delete your own solutions.");
        }

        await _unitOfWork.Solutions.DeleteAsync(solution);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ToggleLikeAsync(int solutionId, string userId)
    {
        await _unitOfWork.Solutions.ToggleLikeAsync(solutionId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ToggleDislikeAsync(int solutionId, string userId)
    {
        await _unitOfWork.Solutions.ToggleDislikeAsync(solutionId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> IsDislikedByUserAsync(int solutionId, string userId)
    {
        return await _unitOfWork.Solutions.IsDislikedByUserAsync(solutionId, userId);
    }

    public async Task AddCommentAsync(int solutionId, string content, string userId)
    {
        await _unitOfWork.Solutions.AddCommentAsync(solutionId, userId, content);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, string userId)
    {
        await _unitOfWork.Solutions.DeleteCommentAsync(commentId, userId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(int solutionId)
    {
        var comments = await _unitOfWork.Solutions.GetCommentsAsync(solutionId);
        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserName = c.User?.UserName
        });
    }

    public async Task<List<SolutionDto>> GetMostLikedSolutionsAsync(int count = 1)
    {
        var solutions = await _unitOfWork.Solutions.GetMostLikedSolutionsAsync(count);
        return _mapper.Map<List<SolutionDto>>(solutions);
    }
}

