using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class ProblemLikeConfiguration : IEntityTypeConfiguration<ProblemLike>
{
    public void Configure(EntityTypeBuilder<ProblemLike> builder)
    {
        builder.HasKey(pl => pl.Id);
        
        builder.Property(pl => pl.ProblemId)
            .IsRequired();
        
        builder.Property(pl => pl.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.HasOne(pl => pl.Problem)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(pl => pl.User)
            .WithMany()
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(pl => new { pl.ProblemId, pl.UserId })
            .IsUnique();
        
        builder.ToTable("ProblemLikes");
    }
}

