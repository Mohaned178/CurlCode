using CurlCode.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.Id);
        
        builder.Property(up => up.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(up => up.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(up => up.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(up => up.Bio)
            .HasMaxLength(500);
        
        builder.Property(up => up.ProfileImageUrl)
            .HasMaxLength(500);
        
        builder.Property(up => up.GitHubUrl)
            .HasMaxLength(200);
        
        builder.Property(up => up.LinkedInUrl)
            .HasMaxLength(200);
        
        builder.Property(up => up.WebsiteUrl)
            .HasMaxLength(200);
        
        builder.Property(up => up.Country)
            .HasMaxLength(100);
        
        builder.Property(up => up.University)
            .HasMaxLength(200);
        
        builder.Property(up => up.Work)
            .HasMaxLength(200);
        
        builder.Property(up => up.Major)
            .HasMaxLength(100);
        
        builder.Property(up => up.TotalSolved)
            .HasDefaultValue(0);
        
        builder.Property(up => up.EasySolved)
            .HasDefaultValue(0);
        
        builder.Property(up => up.MediumSolved)
            .HasDefaultValue(0);
        
        builder.Property(up => up.HardSolved)
            .HasDefaultValue(0);
        
        builder.Property(up => up.TotalScore)
            .HasDefaultValue(0);
        
        builder.Property(up => up.Rank)
            .HasDefaultValue(0);
        
        builder.HasOne(up => up.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(up => up.UserId).IsUnique();
        
        builder.ToTable("UserProfiles");
    }
}






