using CurlCode.Domain.Entities.StudyPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurlCode.Infrastructure.Persistence.Configurations;

public class StudyPlanItemConfiguration : IEntityTypeConfiguration<StudyPlanItem>
{
    public void Configure(EntityTypeBuilder<StudyPlanItem> builder)
    {
        builder.HasKey(spi => spi.Id);

        builder.Property(spi => spi.StudyPlanId)
            .IsRequired();

        builder.Property(spi => spi.ProblemId)
            .IsRequired();

        builder.Property(spi => spi.DayNumber)
            .IsRequired();

        builder.Property(spi => spi.Order)
            .IsRequired();

        builder.HasOne(spi => spi.StudyPlan)
            .WithMany(sp => sp.Items)
            .HasForeignKey(spi => spi.StudyPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(spi => spi.Problem)
            .WithMany()
            .HasForeignKey(spi => spi.ProblemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(spi => new { spi.StudyPlanId, spi.DayNumber, spi.Order });

        builder.ToTable("StudyPlanItems");
    }
}

