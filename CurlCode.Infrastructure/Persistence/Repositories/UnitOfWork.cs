using CurlCode.Application.Contracts.Persistence;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IProblemRepository? _problems;
    private ITopicRepository? _topics;
    private ISubmissionRepository? _submissions;
    private ISolutionRepository? _solutions;
    private IProfileRepository? _profiles;
    private IStudyPlanRepository? _studyPlans;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProblemRepository Problems =>
        _problems ??= new ProblemRepository(_context);

    public ITopicRepository Topics =>
        _topics ??= new TopicRepository(_context);

    public ISubmissionRepository Submissions =>
        _submissions ??= new SubmissionRepository(_context);

    public ISolutionRepository Solutions =>
        _solutions ??= new SolutionRepository(_context);

    public IProfileRepository Profiles =>
        _profiles ??= new ProfileRepository(_context);

    public IStudyPlanRepository StudyPlans =>
        _studyPlans ??= new StudyPlanRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}






