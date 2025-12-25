using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
{
    public void Configure(EntityTypeBuilder<Solution> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.ProblemId)
            .IsRequired();
        
        builder.Property(s => s.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.Content)
            .IsRequired();
        
        builder.Property(s => s.LikesCount)
            .HasDefaultValue(0);
        
        builder.Property(s => s.DislikesCount)
            .HasDefaultValue(0);
        
        builder.Property(s => s.CommentsCount)
            .HasDefaultValue(0);
        
        builder.HasOne(s => s.Problem)
            .WithMany(p => p.Solutions)
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(s => s.User)
            .WithMany(u => u.Solutions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(s => s.Likes)
            .WithOne(sl => sl.Solution)
            .HasForeignKey(sl => sl.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.Dislikes)
            .WithOne(sd => sd.Solution)
            .HasForeignKey(sd => sd.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.Comments)
            .WithOne(c => c.Solution)
            .HasForeignKey(c => c.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(s => s.ProblemId);
        builder.HasIndex(s => s.UserId);
        
        builder.ToTable("Solutions");
    }
}






