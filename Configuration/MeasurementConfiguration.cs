using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueExample.Models;

namespace VueExample.Configuration
{
    public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
    {
        public void Configure(EntityTypeBuilder<Measurement> builder)
        {
            builder.HasMany(c => c.AtomicMeasurement)
                   .WithOne(e => e.Measurement);

            builder.HasMany(c => c.Points)
                   .WithOne(e => e.Measurement);

        }
    }
}