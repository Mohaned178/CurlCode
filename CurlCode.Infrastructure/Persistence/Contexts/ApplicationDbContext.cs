using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Contests;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.StudyPlans;
using CurlCode.Domain.Entities.Submissions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<TestCase> TestCases { get; set; }
    public DbSet<ProblemTopic> ProblemTopics { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Solution> Solutions { get; set; }
    public DbSet<SolutionLike> SolutionLikes { get; set; }
    public DbSet<SolutionDislike> SolutionDislikes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<ProblemLike> ProblemLikes { get; set; }
    public DbSet<ProblemDislike> ProblemDislikes { get; set; }
    public DbSet<ProblemComment> ProblemComments { get; set; }
    public DbSet<StudyPlan> StudyPlans { get; set; }
    public DbSet<StudyPlanItem> StudyPlanItems { get; set; }
    public DbSet<StudyPlanProgress> StudyPlanProgresses { get; set; }
    public DbSet<StudyPlanItemProgress> StudyPlanItemProgresses { get; set; }
    
    // Contest entities
    public DbSet<Contest> Contests { get; set; }
    public DbSet<ContestProblem> ContestProblems { get; set; }
    public DbSet<ContestParticipant> ContestParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all configurations from assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}






