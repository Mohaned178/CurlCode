namespace CurlCode.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IProblemRepository Problems { get; }
    ITopicRepository Topics { get; }
    ISubmissionRepository Submissions { get; }
    ISolutionRepository Solutions { get; }
    IProfileRepository Profiles { get; }
    IStudyPlanRepository StudyPlans { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}






