using CurlCode.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class SolutionDislikeConfiguration : IEntityTypeConfiguration<SolutionDislike>
{
    public void Configure(EntityTypeBuilder<SolutionDislike> builder)
    {
        builder.HasKey(sd => sd.Id);
        
        builder.Property(sd => sd.SolutionId)
            .IsRequired();
        
        builder.Property(sd => sd.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.HasOne(sd => sd.Solution)
            .WithMany(s => s.Dislikes)
            .HasForeignKey(sd => sd.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sd => sd.User)
            .WithMany()
            .HasForeignKey(sd => sd.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(sd => new { sd.SolutionId, sd.UserId })
            .IsUnique();
        
        builder.ToTable("SolutionDislikes");
    }
}

