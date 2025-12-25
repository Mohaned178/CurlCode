using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class ProblemCommentConfiguration : IEntityTypeConfiguration<ProblemComment>
{
    public void Configure(EntityTypeBuilder<ProblemComment> builder)
    {
        builder.HasKey(pc => pc.Id);
        
        builder.Property(pc => pc.ProblemId)
            .IsRequired();
        
        builder.Property(pc => pc.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(pc => pc.Content)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.HasOne(pc => pc.Problem)
            .WithMany(p => p.Comments)
            .HasForeignKey(pc => pc.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(pc => pc.User)
            .WithMany()
            .HasForeignKey(pc => pc.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(pc => pc.ProblemId);
        builder.HasIndex(pc => pc.UserId);
        
        builder.ToTable("ProblemComments");
    }
}

