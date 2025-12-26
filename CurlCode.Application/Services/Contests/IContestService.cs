using CurlCode.Application.DTOs.Contests;

namespace CurlCode.Application.Services.Contests;

public interface IContestService
{
    Task<ContestDetailDto?> GetContestByIdAsync(int id);
    Task<IEnumerable<ContestDto>> GetUpcomingContestsAsync();
    Task<IEnumerable<ContestDto>> GetRunningContestsAsync();
    Task<IEnumerable<ContestDto>> GetPastContestsAsync(int count = 10);
    Task<ContestDto> CreateContestAsync(CreateContestRequest request);
    Task<ContestDto?> UpdateContestAsync(int id, UpdateContestRequest request);
    Task<bool> DeleteContestAsync(int id);
    Task<bool> JoinContestAsync(int contestId, string userId);
    Task<IEnumerable<ContestStandingDto>> GetStandingsAsync(int contestId);
}
