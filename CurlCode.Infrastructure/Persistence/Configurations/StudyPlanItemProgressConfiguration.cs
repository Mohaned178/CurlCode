using CurlCode.Domain.Entities.StudyPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class StudyPlanItemProgressConfiguration : IEntityTypeConfiguration<StudyPlanItemProgress>
{
    public void Configure(EntityTypeBuilder<StudyPlanItemProgress> builder)
    {
        builder.HasKey(spp => spp.Id);

        builder.Property(spp => spp.StudyPlanProgressId)
            .IsRequired();

        builder.Property(spp => spp.StudyPlanItemId)
            .IsRequired();

        builder.Property(spp => spp.IsCompleted)
            .HasDefaultValue(false);

        builder.HasOne(spp => spp.StudyPlanProgress)
            .WithMany(spp => spp.ItemProgresses)
            .HasForeignKey(spp => spp.StudyPlanProgressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(spp => spp.StudyPlanItem)
            .WithMany()
            .HasForeignKey(spp => spp.StudyPlanItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(spp => new { spp.StudyPlanProgressId, spp.StudyPlanItemId })
            .IsUnique();

        builder.ToTable("StudyPlanItemProgresses");
    }
}

