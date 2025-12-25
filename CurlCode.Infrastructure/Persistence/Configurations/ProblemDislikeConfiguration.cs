using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class ProblemDislikeConfiguration : IEntityTypeConfiguration<ProblemDislike>
{
    public void Configure(EntityTypeBuilder<ProblemDislike> builder)
    {
        builder.HasKey(pd => pd.Id);
        
        builder.Property(pd => pd.ProblemId)
            .IsRequired();
        
        builder.Property(pd => pd.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.HasOne(pd => pd.Problem)
            .WithMany(p => p.Dislikes)
            .HasForeignKey(pd => pd.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(pd => pd.User)
            .WithMany()
            .HasForeignKey(pd => pd.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(pd => new { pd.ProblemId, pd.UserId })
            .IsUnique();
        
        builder.ToTable("ProblemDislikes");
    }
}

