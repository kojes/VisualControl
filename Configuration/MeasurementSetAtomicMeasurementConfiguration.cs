using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueExample.Models;

namespace VueExample.Configuration
{
    public class MeasurementSetAtomicMeasurementConfiguration : IEntityTypeConfiguration<MeasurementSetAtomicMeasurement>
    {
        public void Configure(EntityTypeBuilder<MeasurementSetAtomicMeasurement> builder)
        {
            builder.HasKey(t => new { t.AtomicMeasurementId, t.MeasurementSetId });

            builder.HasOne(pt => pt.MeasurementSet)
                   .WithMany(p => p.MeasurementSetAtomicMeasurement)
                   .HasForeignKey(pt => pt.MeasurementSetId);

            builder.HasOne(pt => pt.AtomicMeasurement)
                   .WithMany(p => p.MeasurementSetAtomicMeasurement)
                   .HasForeignKey(pt => pt.AtomicMeasurementId);
        }
    }
}