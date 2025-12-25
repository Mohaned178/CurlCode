using CurlCode.Domain.Entities.Submissions;
using CurlCode.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.ProblemId)
            .IsRequired();
        
        builder.Property(s => s.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(s => s.Code)
            .IsRequired();
        
        builder.Property(s => s.Language)
            .IsRequired();
        
        builder.Property(s => s.Status)
            .IsRequired()
            .HasDefaultValue(SubmissionStatus.Pending);
        
        builder.Property(s => s.ErrorMessage)
            .HasMaxLength(1000);
        
        builder.HasOne(s => s.Problem)
            .WithMany(p => p.Submissions)
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(s => s.User)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(s => s.ProblemId);
        builder.HasIndex(s => s.UserId);
        builder.HasIndex(s => new { s.ProblemId, s.UserId });
        builder.HasIndex(s => s.Status);
        
        builder.ToTable("Submissions");
    }
}

