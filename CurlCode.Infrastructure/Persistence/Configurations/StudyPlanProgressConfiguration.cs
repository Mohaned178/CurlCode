using CurlCode.Domain.Entities.StudyPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class StudyPlanProgressConfiguration : IEntityTypeConfiguration<StudyPlanProgress>
{
    public void Configure(EntityTypeBuilder<StudyPlanProgress> builder)
    {
        builder.HasKey(spp => spp.Id);

        builder.Property(spp => spp.StudyPlanId)
            .IsRequired();

        builder.Property(spp => spp.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(spp => spp.CompletedProblems)
            .HasDefaultValue(0);

        builder.HasOne(spp => spp.StudyPlan)
            .WithMany(sp => sp.Progresses)
            .HasForeignKey(spp => spp.StudyPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(spp => spp.User)
            .WithMany()
            .HasForeignKey(spp => spp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(spp => spp.ItemProgresses)
            .WithOne(spp => spp.StudyPlanProgress)
            .HasForeignKey(spp => spp.StudyPlanProgressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(spp => new { spp.StudyPlanId, spp.UserId })
            .IsUnique();

        builder.ToTable("StudyPlanProgresses");
    }
}

