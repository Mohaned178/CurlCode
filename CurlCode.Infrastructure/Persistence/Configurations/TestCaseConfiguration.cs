using CurlCode.Domain.Entities.Problems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> builder)
    {
        builder.HasKey(tc => tc.Id);
        
        builder.Property(tc => tc.ProblemId)
            .IsRequired();
        
        builder.Property(tc => tc.Input)
            .IsRequired();
        
        builder.Property(tc => tc.ExpectedOutput)
            .IsRequired();
        
        builder.Property(tc => tc.IsHidden)
            .HasDefaultValue(true);
        
        builder.Property(tc => tc.Order)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.HasOne(tc => tc.Problem)
            .WithMany(p => p.TestCases)
            .HasForeignKey(tc => tc.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(tc => new { tc.ProblemId, tc.Order });
        
        builder.ToTable("TestCases");
    }
}

