using CurlCode.Domain.Entities.Problems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(t => t.Description)
            .HasMaxLength(500);
        
        builder.HasIndex(t => t.Name).IsUnique();
        
        builder.HasMany(t => t.ProblemTopics)
            .WithOne(pt => pt.Topic)
            .HasForeignKey(pt => pt.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable("Topics");
    }
}

