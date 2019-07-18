using Microsoft.EntityFrameworkCore;
using VueExample.Models;
using VueExample.Configuration;
using Microsoft.Extensions.Configuration;

namespace VueExample.Contexts
{
    public class ApplicationContext : DbContext 
    {        
        public DbSet<Models.Point> Point { get; set; }
        public DbSet<Graphic> Graphic { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<MeasuredDevice> MeasuredDevice { get; set; }
        public DbSet<AtomicMeasurement> AtomicMeasurement { get; set; }
        public DbSet<MeasurementSet> MeasurementSet { get; set; }
        public DbSet<MeasurementSetAtomicMeasurement> MeasurementSetAtomicMeasurement { get; set; }
      
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            
        }
           
        protected override void OnModelCreating (ModelBuilder modelBuilder) {

            modelBuilder.ApplyConfiguration(new MeasurementSetAtomicMeasurementConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new MeasurementConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new GraphicConfiguration());
                     
        }
    }
}