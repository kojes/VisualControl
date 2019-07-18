using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueExample.Models;

namespace VueExample.Configuration
{
    public class GraphicConfiguration : IEntityTypeConfiguration<Graphic>
    {
        public void Configure(EntityTypeBuilder<Graphic> builder)
        {
            builder.HasMany(c => c.AtomicMeasurement)
                   .WithOne(e => e.Graphic);
        }
    }
}