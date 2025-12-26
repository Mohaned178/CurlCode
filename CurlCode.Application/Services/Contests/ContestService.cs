using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Contests;
using CurlCode.Domain.Entities.Contests;
using CurlCode.Domain.Enums;

namespace CurlCode.Application.Services.Contests;

public class ContestService : IContestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContestService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContestDetailDto?> GetContestByIdAsync(int id)
    {
        var contest = await _unitOfWork.Contests.GetByIdWithDetailsAsync(id);
        if (contest == null) return null;

        return new ContestDetailDto
        {
            Id = contest.Id,
            Title = contest.Title,
            Description = contest.Description,
            StartTime = contest.StartTime,
            EndTime = contest.EndTime,
            Status = contest.Status,
            IsPublic = contest.IsPublic,
            ParticipantCount = contest.Participants.Count,
            ProblemCount = contest.ContestProblems.Count,
            Problems = contest.ContestProblems.Select(cp => new ContestProblemDto
            {
                ProblemId = cp.ProblemId,
                Title = cp.Problem?.Title ?? string.Empty,
                Order = cp.Order,
                Points = cp.Points,
                Difficulty = cp.Problem?.Difficulty ?? DifficultyLevel.Easy
            }).OrderBy(p => p.Order)
        };
    }

    public async Task<IEnumerable<ContestDto>> GetUpcomingContestsAsync()
    {
        var contests = await _unitOfWork.Contests.GetUpcomingContestsAsync();
        return _mapper.Map<IEnumerable<ContestDto>>(contests);
    }

    public async Task<IEnumerable<ContestDto>> GetRunningContestsAsync()
    {
        var contests = await _unitOfWork.Contests.GetRunningContestsAsync();
        return _mapper.Map<IEnumerable<ContestDto>>(contests);
    }

    public async Task<IEnumerable<ContestDto>> GetPastContestsAsync(int count = 10)
    {
        var contests = await _unitOfWork.Contests.GetPastContestsAsync(count);
        return _mapper.Map<IEnumerable<ContestDto>>(contests);
    }

    public async Task<ContestDto> CreateContestAsync(CreateContestRequest request)
    {
        var contest = new Contest
        {
            Title = request.Title,
            Description = request.Description,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            IsPublic = request.IsPublic,
            Status = ContestStatus.Upcoming
        };

        // Add problems to contest
        int order = 0;
        foreach (var problemId in request.ProblemIds)
        {
            contest.ContestProblems.Add(new ContestProblem
            {
                ProblemId = problemId,
                Order = order++,
                Points = 100
            });
        }

        await _unitOfWork.Contests.AddAsync(contest);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ContestDto>(contest);
    }

    public async Task<ContestDto?> UpdateContestAsync(int id, UpdateContestRequest request)
    {
        var contest = await _unitOfWork.Contests.GetByIdAsync(id);
        if (contest == null) return null;

        if (request.Title != null) contest.Title = request.Title;
        if (request.Description != null) contest.Description = request.Description;
        if (request.StartTime.HasValue) contest.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue) contest.EndTime = request.EndTime.Value;
        if (request.IsPublic.HasValue) contest.IsPublic = request.IsPublic.Value;

        await _unitOfWork.Contests.UpdateAsync(contest);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ContestDto>(contest);
    }

    public async Task<bool> DeleteContestAsync(int id)
    {
        var contest = await _unitOfWork.Contests.GetByIdAsync(id);
        if (contest == null) return false;

        await _unitOfWork.Contests.DeleteAsync(contest);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> JoinContestAsync(int contestId, string userId)
    {
        var contest = await _unitOfWork.Contests.GetByIdAsync(contestId);
        if (contest == null)
            throw new NotFoundException(nameof(Contest), contestId);

        if (contest.Status != ContestStatus.Upcoming && contest.Status != ContestStatus.Running)
            throw new ValidationException("Cannot join a contest that has already ended.");

        var existingParticipant = await _unitOfWork.Contests.GetParticipantAsync(contestId, userId);
        if (existingParticipant != null)
            return false; // Already registered

        var participant = new ContestParticipant
        {
            ContestId = contestId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };

        await _unitOfWork.Contests.AddParticipantAsync(participant);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ContestStandingDto>> GetStandingsAsync(int contestId)
    {
        var participants = await _unitOfWork.Contests.GetStandingsAsync(contestId);
        int rank = 1;
        return participants.Select(p => new ContestStandingDto
        {
            Rank = rank++,
            UserId = p.UserId,
            UserName = p.User?.UserName ?? string.Empty,
            Score = p.Score,
            SolvedCount = p.SolvedCount,
            Penalty = p.Penalty
        });
    }
}
