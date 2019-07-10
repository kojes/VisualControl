using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Models;

namespace VueExample.Contexts {
    public class ApplicationContext : DbContext {
        public DbSet<Models.Point> Point { get; set; }
        public DbSet<Graphic> Graphic { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<MeasuredDevice> MeasuredDevice { get; set; }
        public DbSet<AtomicMeasurement> AtomicMeasurement { get; set; }
        public DbSet<MeasurementSet> MeasurementSet { get; set; }
        public DbSet<MeasurementSetAtomicMeasurement> MeasurementSetAtomicMeasurement { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer (@"data source = SRV3\SRV3; Initial Catalog = CrossTesting; persist security info = True; user id = vcu; password = zxvitr78KK; MultipleActiveResultSets = True;");
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<MeasurementSetAtomicMeasurement> ()
                .HasKey (t => new { t.AtomicMeasurementId, t.MeasurementSetId });

            modelBuilder.Entity<MeasurementSetAtomicMeasurement> ()
                .HasOne (pt => pt.MeasurementSet)
                .WithMany (p => p.MeasurementSetAtomicMeasurement)
                .HasForeignKey (pt => pt.MeasurementSetId);

            modelBuilder.Entity<MeasurementSetAtomicMeasurement> ()
                .HasOne (pt => pt.AtomicMeasurement)
                .WithMany (p => p.MeasurementSetAtomicMeasurement)
                .HasForeignKey (pt => pt.AtomicMeasurementId);

            modelBuilder.Entity<Device> ()
                .HasMany (c => c.AtomicMeasurement)
                .WithOne (e => e.Device);

            modelBuilder.Entity<Measurement> ()
                .HasMany (c => c.AtomicMeasurement)
                .WithOne (e => e.Measurement);

            modelBuilder.Entity<Measurement> ()
                .HasMany (c => c.Points)
                .WithOne (e => e.Measurement);

            modelBuilder.Entity<Material> ()
                .HasMany (c => c.Measurements)
                .WithOne (e => e.Material);

            modelBuilder.Entity<Graphic> ()
                .HasMany (c => c.AtomicMeasurement)
                .WithOne (e => e.Graphic);
        }
    }
}