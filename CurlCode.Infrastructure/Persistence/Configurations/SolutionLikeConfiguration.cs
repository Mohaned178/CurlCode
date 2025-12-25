using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class SolutionLikeConfiguration : IEntityTypeConfiguration<SolutionLike>
{
    public void Configure(EntityTypeBuilder<SolutionLike> builder)
    {
        builder.HasKey(sl => sl.Id);
        
        builder.Property(sl => sl.SolutionId)
            .IsRequired();
        
        builder.Property(sl => sl.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.HasOne(sl => sl.Solution)
            .WithMany(s => s.Likes)
            .HasForeignKey(sl => sl.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sl => sl.User)
            .WithMany(u => u.SolutionLikes)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(sl => new { sl.SolutionId, sl.UserId })
            .IsUnique();
        
        builder.ToTable("SolutionLikes");
    }
}

