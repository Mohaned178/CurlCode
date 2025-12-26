using CurlCode.Domain.Entities.Contests;

namespace CurlCode.Application.Contracts.Persistence;

public interface IContestRepository : IGenericRepository<Contest>
{
    Task<Contest?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Contest>> GetUpcomingContestsAsync();
    Task<IEnumerable<Contest>> GetRunningContestsAsync();
    Task<IEnumerable<Contest>> GetPastContestsAsync(int count = 10);
    Task<IEnumerable<ContestParticipant>> GetStandingsAsync(int contestId);
    Task<ContestParticipant?> GetParticipantAsync(int contestId, string userId);
    Task AddParticipantAsync(ContestParticipant participant);
    Task UpdateParticipantAsync(ContestParticipant participant);
}
