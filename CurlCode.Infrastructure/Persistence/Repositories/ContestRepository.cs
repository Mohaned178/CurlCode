using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Contests;
using CurlCode.Domain.Enums;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class ContestRepository : GenericRepository<Contest>, IContestRepository
{
    public ContestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Contest?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.ContestProblems)
                .ThenInclude(cp => cp.Problem)
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Contest>> GetUpcomingContestsAsync()
    {
        return await _dbSet
            .Where(c => c.Status == ContestStatus.Upcoming)
            .OrderBy(c => c.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contest>> GetRunningContestsAsync()
    {
        return await _dbSet
            .Where(c => c.Status == ContestStatus.Running)
            .OrderBy(c => c.EndTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contest>> GetPastContestsAsync(int count = 10)
    {
        return await _dbSet
            .Where(c => c.Status == ContestStatus.Ended)
            .OrderByDescending(c => c.EndTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<ContestParticipant>> GetStandingsAsync(int contestId)
    {
        return await _context.ContestParticipants
            .Include(cp => cp.User)
            .Where(cp => cp.ContestId == contestId)
            .OrderByDescending(cp => cp.Score)
            .ThenBy(cp => cp.Penalty)
            .ToListAsync();
    }

    public async Task<ContestParticipant?> GetParticipantAsync(int contestId, string userId)
    {
        return await _context.ContestParticipants
            .FirstOrDefaultAsync(cp => cp.ContestId == contestId && cp.UserId == userId);
    }

    public async Task AddParticipantAsync(ContestParticipant participant)
    {
        await _context.ContestParticipants.AddAsync(participant);
    }

    public async Task UpdateParticipantAsync(ContestParticipant participant)
    {
        _context.ContestParticipants.Update(participant);
        await Task.CompletedTask;
    }
}
