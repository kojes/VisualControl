using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VueExample.Entities.Configurations
{
    public class StandartMeasurementPatternConfiguration : IEntityTypeConfiguration<StandartMeasurementPatternEntity>
    {
        public void Configure(EntityTypeBuilder<StandartMeasurementPatternEntity> builder)
        {
            builder.HasOne(k => k.Divider).WithMany().HasForeignKey(fk => fk.DividerId);
            builder.HasMany(k => k.KurbatovParameters).WithOne(k => k.StandartMeasurementPatternEntity).HasForeignKey(fk => fk.SmpId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(k => k.Stage).WithMany().HasForeignKey(fk => fk.StageId);
            builder.HasOne(k => k.Element).WithMany().HasForeignKey(fk => fk.ElementId);
            builder.HasOne(k => k.StandartPattern).WithMany(p => p.StandartMeasurementPatterns).HasForeignKey(fk => fk.PatternId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}