using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.SolutionId)
            .IsRequired();
        
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.HasOne(c => c.Solution)
            .WithMany(s => s.Comments)
            .HasForeignKey(c => c.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(c => c.SolutionId);
        builder.HasIndex(c => c.UserId);
        
        builder.ToTable("Comments");
    }
}

