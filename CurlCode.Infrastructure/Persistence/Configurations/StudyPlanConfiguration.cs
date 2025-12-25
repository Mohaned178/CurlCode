using CurlCode.Domain.Entities.StudyPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class StudyPlanConfiguration : IEntityTypeConfiguration<StudyPlan>
{
    public void Configure(EntityTypeBuilder<StudyPlan> builder)
    {
        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sp => sp.Description)
            .IsRequired();

        builder.Property(sp => sp.Difficulty)
            .IsRequired();

        builder.Property(sp => sp.DurationDays)
            .IsRequired();

        builder.Property(sp => sp.TotalProblems)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(sp => sp.IsActive)
            .HasDefaultValue(true);

        builder.Property(sp => sp.IsCustom)
            .HasDefaultValue(false);

        builder.Property(sp => sp.UserId)
            .HasMaxLength(450);

        builder.HasOne(sp => sp.User)
            .WithMany()
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(sp => sp.Items)
            .WithOne(spi => spi.StudyPlan)
            .HasForeignKey(spi => spi.StudyPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sp => sp.Progresses)
            .WithOne(spp => spp.StudyPlan)
            .HasForeignKey(spp => spp.StudyPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("StudyPlans");
    }
}

