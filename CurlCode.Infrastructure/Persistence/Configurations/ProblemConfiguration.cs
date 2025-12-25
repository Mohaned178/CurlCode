using CurlCode.Domain.Entities.Problems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Description)
            .IsRequired();
        
        builder.Property(p => p.Difficulty)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
        
        builder.Property(p => p.TimeLimitMs)
            .HasDefaultValue(2000);
        
        builder.Property(p => p.MemoryLimitMb)
            .HasDefaultValue(256);
        
        builder.Property(p => p.TotalSubmissions)
            .HasDefaultValue(0);
        
        builder.Property(p => p.AcceptedSubmissions)
            .HasDefaultValue(0);
        
        builder.Property(p => p.LikesCount)
            .HasDefaultValue(0);
        
        builder.Property(p => p.DislikesCount)
            .HasDefaultValue(0);
        
        builder.Property(p => p.CommentsCount)
            .HasDefaultValue(0);

        builder.HasMany(p => p.TestCases)
            .WithOne(tc => tc.Problem)
            .HasForeignKey(tc => tc.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Dislikes)
            .WithOne(pd => pd.Problem)
            .HasForeignKey(pd => pd.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Comments)
            .WithOne(pc => pc.Problem)
            .HasForeignKey(pc => pc.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Submissions)
            .WithOne(s => s.Problem)
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.Solutions)
            .WithOne(s => s.Problem)
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => p.Difficulty);
        
        builder.ToTable("Problems");
    }
}






